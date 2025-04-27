using Bl.Agrobook.Financial.Func.Services;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;

namespace Bl.Agrobook.Financial.Func.Functions;

internal class FinancialPdfFunction
{
    private readonly AuthService _authService;
    private readonly ILogger _logger;
    private readonly FinancialApiService _api;

    public FinancialPdfFunction(
        AuthService authService,
        FinancialApiService api,
        ILogger<FinancialPdfFunction> logger)
    {
        _authService = authService;
        _logger = logger;
        _api = api;
    }

    [Function("GeneratePdf")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "financial/order/pdf")] HttpRequest req,
        CancellationToken cancellationToken = default)
    {
        if (!_authService.IsAuthenticated(req))
        {
            return new UnauthorizedResult();
        }

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = System.Text.Json.JsonSerializer.Deserialize<JsonNode>(requestBody);

            var date = data?["orderDate"]?.ToString();

            if (string.IsNullOrEmpty(date)) date = $"{DateTime.Now.ToString("dd/MM/yyyy")}";

            var orders = await _api.GetOrdersAsync().ToListAsync(cancellationToken);

            if (orders.Count == 0) return new OkObjectResult("No orders found.");

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

            foreach (var order in orders.OrderBy(o => o.Products.Count))
            {
                // Create a cell for the left or right side
                var cell = new Cell()
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
                    var listItem = new ListItem($"□ {product.Description} - {product.Qty?.ToString("0")}");
                    listItem.SetFontSize(10);

                    list.Add(listItem);
                }

                cell.Add(list);

                // Add the cell to the table
                table.AddCell(cell);
            }

            // Add the table to the document
            document.Add(table);

            document.Close();

            await Task.CompletedTask;

            return new FileContentResult(memoryStream.ToArray(), "application/pdf")
            {
                FileDownloadName = $"Pedidos-{DateTime.Now:yyyy-MM-dd}.pdf"
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
}
