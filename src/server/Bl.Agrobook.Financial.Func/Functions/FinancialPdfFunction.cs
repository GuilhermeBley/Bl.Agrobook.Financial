using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Services;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bl.Agrobook.Financial.Func.Functions;

internal class FinancialPdfFunction
{
    private readonly AuthService _authService;
    private readonly ILogger _logger;
    private readonly FinancialApiService _api;
    private readonly SalesService _salesService;

    public FinancialPdfFunction(
        AuthService authService,
        FinancialApiService api,
        ILogger<FinancialPdfFunction> logger,
        SalesService salesService)
    {
        _authService = authService;
        _logger = logger;
        _api = api;
        _salesService = salesService;
    }

    [Function("GeneratePdfByFiles")]
    public async Task<IActionResult> RunByFiles(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "financial/order/pdf/by-file")] HttpRequest req,
        CancellationToken cancellationToken = default)
    {
        if (!_authService.IsAuthenticated(req))
        {
            return new UnauthorizedResult();
        }

        if (!req.HasFormContentType || req.Form.Files.Count == 0)
        {
            return new BadRequestObjectResult("No file uploaded.");
        }

        var file = req.Form.Files[0];

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var date = req.Query["orderDate"];
            var orderRequisitionDate = req.Query["orderCreatedAt"];

            if (!DateTime.TryParseExact(orderRequisitionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var orderRequisition))
            {
                return new BadRequestObjectResult(new
                {
                    Message = "Adicione a data 'orderCreatedAt' no corpo da requisição."
                });
            }
            var cultureInfoQuery = req.Query["cultureInfo"].ToString();

            if (string.IsNullOrWhiteSpace(cultureInfoQuery)) cultureInfoQuery = "pt-BR";

            CultureInfo cultureInfo = new(cultureInfoQuery);

            var ordersToCreate = await _salesService.CheckFileAsync(cultureInfo, file, cancellationToken);

            var products = await _api.GetProductsAsync().ToListAsync(cancellationToken);

            if (products.Count == 0)
            {
                return new NotFoundObjectResult("No products found.");
            }

            _logger.LogInformation("Products: {Products}", products.Count);

            var customers = await _api.GetCustomersAsync().ToListAsync(cancellationToken);

            if (customers.Count == 0)
            {
                return new NotFoundObjectResult("No customers found.");
            }

            _logger.LogInformation("Customers: {customers}", customers.Count);

            var createModels = _salesService
                .MapOrdersByFormFileAsync(cultureInfo, ordersToCreate, products, customers, cancellationToken)
                .ToList();

            using var memoryStream = new MemoryStream();

            GenerateByOrdersAsync(
                memoryStream, 
                date,
                createModels
                .Select(x => new SaleHistoryViewModel
                {
                    Canceled = false,
                    Code = $"G-{orderRequisition.ToString("yyyyMMdd")}{createModels.IndexOf(x)}",
                    FinalValue = x.FinalValue,
                    Products = x.Products.Select(p => new SaleProduct
                    {
                        Code = p.Code ?? string.Empty,
                        Description = p.Description,
                        Qty = p.Qty,
                        FinalValue = p.FinalValue,
                        Obs = p.Obs
                    }).ToList(),
                    Customer = new CustomerViewModel
                    {
                        Name = x.Customer.Name,
                        Uid = x.Customer.Uid
                    },
                    Obs = x.Obs,
                }).ToArray());

            File.WriteAllBytes("C:\\Users\\guilh\\Downloads\\Pedidos-2025-10-08.pdf", memoryStream.ToArray());

            return new FileContentResult(memoryStream.ToArray(), "application/pdf")
            {
                FileDownloadName = $"Pedidos-{date:yyyy-MM-dd}.pdf"
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the request.");
            return new BadRequestObjectResult(new
            {
                e.Message
            });
        }
    }

    [Function("GeneratePdf")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "financial/order/pdf")] HttpRequest req,
        CancellationToken cancellationToken = default)
    {
        if (!_authService.IsAuthenticated(req))
        {
            return new UnauthorizedResult();
        }

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            JsonNode? data = null;
            try
            {
                data = System.Text.Json.JsonSerializer.Deserialize<JsonNode>(requestBody);
            }
            catch { }

            var date = data?["orderDate"]?.ToString();
            var orderRequisitionDate = data?["orderCreatedAt"]?.ToString();

            if (!DateTime.TryParseExact(orderRequisitionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var orderRequisition))
            {
                return new BadRequestObjectResult(new
                {
                    Message = "Adicione a data 'orderCreatedAt' no corpo da requisição."
                });
            }

            if (string.IsNullOrEmpty(date)) date = $"{DateTime.Now.ToString("dd/MM/yyyy")}";

            var orders = await _api.GetOrdersAsync(datebegin: orderRequisition).ToArrayAsync(cancellationToken);
            orders = orders.Where(x => x.Date >= orderRequisition).ToArray();

            if (orders.Length == 0) return new NoContentResult();

            using var memoryStream = new MemoryStream();

            GenerateByOrdersAsync(memoryStream, date, orders);
            
            return new FileContentResult(memoryStream.ToArray(), "application/pdf")
            {
                FileDownloadName = $"Pedidos-{date:yyyy-MM-dd}.pdf"
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the request.");
            return new BadRequestObjectResult(new
            {
                e.Message
            });
        }
    }

    /// <summary>
    /// Generates a PDF document with orders displayed in a two-side layout. The generation will be made in a MemoryStream.
    /// </summary>
    private void GenerateByOrdersAsync(
        MemoryStream memoryStream,
        string? date,
        SaleHistoryViewModel[] orders)
    {

        // Initialize PDF writer and document
        var writer = new PdfWriter(memoryStream);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        // Add content to the PDF
        document.Add(new Paragraph($"Pedidos para {date}")
            .SetFontSize(20)
            .SimulateBold()
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

        // Create a table with two columns for the two-side layout
        var table = new Table(2).UseAllAvailableWidth();
        Dictionary<string, Model.SaleHistoryViewModel> ordersAdded = new(StringComparer.OrdinalIgnoreCase);

        foreach (var order in orders.OrderBy(o => o.Products.Count))
        {
            if (!ordersAdded.TryAdd(order.Code, order))
            {
                _logger.LogInformation("Code {0} already added.", order.Code);
                continue;
            }

            // Create a cell for the left or right side
            var cell = new Cell()
                .SetKeepTogether(true)
                .SetPadding(10)
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            // Add order title
            cell.Add(new Paragraph($"{order.Customer.Name} ({order.Code})")
                .SetFontSize(11)
                .SimulateBold());

            // Add order items
            var list = new List()
                .SetListSymbol("\u2022");

            foreach (var product in order.Products)
            {
                var listItem = new ListItem(
                    $"□ {product.Description} - {product.Qty?.ToString("0")}" +
                    (string.IsNullOrWhiteSpace(product.Obs) ? "" : $" ({product.Obs})"));
                listItem.SetFontSize(10);
                list.Add(listItem);
            }

            cell.Add(list);

            order.Obs = order.Obs?.Replace("Venda isenta de impostos ", "");

            if (!string.IsNullOrEmpty(order.Obs) &&
                !order.Obs.Contains("Venda isenta de impostos", StringComparison.OrdinalIgnoreCase))
            {
                cell.Add(new Paragraph($"Obs: {order.Obs}")
                    .SetFontSize(9)
                    .SetMarginTop(5));
            }

            // Add the cell to the table
            table.AddCell(cell);
        }

        // Add the table to the document
        document.Add(table);

        // Add the summary table to the document
        if (ordersAdded.Count > 0)
        {
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            var summaryTable = CreateQuantitySalesSummary(ordersAdded.Values);
            document.Add(new Paragraph($"Totais de produtos pedidos")
            .SetFontSize(20)
            .SimulateBold()
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)); // title sales summary
            document.Add(summaryTable);
        }

        document.Close();
    }

    private static Table CreateQuantitySalesSummary(IEnumerable<Model.SaleHistoryViewModel> sales)
    {
        var productsSummary = sales
                .SelectMany(x => x.Products)
                .GroupBy(x => new { x.Code, x.Description })
                .Select(g => new
                {
                    g.Key.Code,
                    g.Key.Description,
                    TotalQty = g.Sum(p => p.Qty),
                    TotalValue = g.Sum(p => p.FinalValue),
                })
                .OrderBy(x => x.Description)
                .ToArray();

        var chuncks = productsSummary.Chunk(productsSummary.Length/2+1);
        var tableView = new Table(2).UseAllAvailableWidth();
        foreach (var pSummaries in chuncks)
        {
            var summaryTable = new Table(2).SetFontSize(8)
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            summaryTable.AddHeaderCell(new Cell().Add(new Paragraph("Produto").SimulateBold()))
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            summaryTable.AddHeaderCell(new Cell().Add(new Paragraph("Quantidade").SimulateBold()))
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            foreach (var product in pSummaries)
            {
                var cell = new Cell()
                    .SetKeepTogether(true)
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                var cell2 = new Cell()
                    .SetKeepTogether(true)
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                var description = string.Concat(product.Description?.Take(60) ?? string.Empty);
                cell.Add(new Paragraph(description));
                cell2.Add(new Paragraph(((int?)product.TotalQty).ToString()));
                summaryTable.AddCell(cell);
                summaryTable.AddCell(cell2);
            }

            var cellView = new Cell().SetKeepTogether(true);
            cellView.Add(summaryTable);
            tableView.AddCell(cellView);
        }


        return tableView;
    }
}
