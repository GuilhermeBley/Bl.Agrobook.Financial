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

namespace Bl.Agrobook.Financial.Func.Functions;

internal class FinancialPdfFunctionV2
{
    private readonly AuthService _authService;
    private readonly ILogger _logger;
    private readonly FinancialKyteApiService _api;

    public FinancialPdfFunctionV2(
        AuthService authService,
        FinancialKyteApiService api,
        ILogger<FinancialPdfFunctionV2> logger)
    {
        _authService = authService;
        _logger = logger;
        _api = api;
    }

    [Function("GeneratePdfV2")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v2/financial/order/pdf")] HttpRequest req,
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

            var orders = await _api.GetPendingSalesAsync(startDate: orderRequisition).ToListAsync(cancellationToken);

            if (orders.Count == 0) return new NoContentResult();

            using var memoryStream = new MemoryStream();

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
            Dictionary<string, Model.Kyte.GetSaleModel> ordersAdded = new(StringComparer.OrdinalIgnoreCase);

            foreach (var order in orders
                .Where(o => o.Active is true)
                .Where(o => o.IsCancelled is false)
                .OrderBy(o => o.Items.Count))
            {
                var code = order.Number.ToString();
                if (!ordersAdded.TryAdd(code, order))
                {
                    _logger.LogInformation("Code {0} already added.", code);
                    continue;
                }

                // Create a cell for the left or right side
                var cell = new Cell()
                    .SetKeepTogether(true)
                    .SetPadding(10)
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                // Add order title
                cell.Add(new Paragraph($"{order.Customer?.Name} ({code})")
                    .SetFontSize(11)
                    .SimulateBold());

                // Add order items
                var list = new List()
                    .SetListSymbol("\u2022");

                foreach (var product in order.Items)
                {
                    var listItem = new ListItem(
                        $"□ {product.Product.Name} - {product.Amount?.ToString("0")}");
                    listItem.SetFontSize(10);
                    list.Add(listItem);
                }

                cell.Add(list);

                if (!string.IsNullOrEmpty(order.Observation))
                {
                    cell.Add(new Paragraph($"Obs: {order.Observation}")
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

    private static Table CreateQuantitySalesSummary(IEnumerable<Model.Kyte.GetSaleModel> sales)
    {
        var productsSummary = sales
                .SelectMany(x => x.Items)
                .GroupBy(x => new { x.Product.ProdId, x.Product.Code, x.Product.Name })
                .Select(g => new
                {
                    g.Key.Code,
                    g.Key.Name,
                    TotalQty = g.Sum(p => p.Amount),
                    TotalValue = g.Sum(p => p.Value),
                })
                .OrderBy(x => x.Name)
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
                var description = string.Concat(product.Name?.Take(60) ?? string.Empty);
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
