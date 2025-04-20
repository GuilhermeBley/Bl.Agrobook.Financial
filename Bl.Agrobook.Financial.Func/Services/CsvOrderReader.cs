using Bl.Agrobook.Financial.Func.Model;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Bl.Agrobook.Financial.Func.Services;

public class CsvOrderReader
{
    private readonly static CultureInfo cultureInfo = new("pt-BR");

    private readonly ILogger<CsvOrderReader> _logger;

    public CsvOrderReader(ILogger<CsvOrderReader> logger)
    {
        _logger = logger;
    }

    public async Task<CreateOrderCsvModel[]> MapCreateOrderCsvAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        try
        {
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, new CsvConfiguration(cultureInfo)
            {
                HasHeaderRecord = true, // Set to false if no header row
                MissingFieldFound = null, // Ignore missing fields
                BadDataFound = context =>
                {
                    _logger.LogWarning($"Bad data found at row {context.Field}: {context.RawRecord}");
                }
            });

            csv.Context.RegisterClassMap<InternalOrderCsvMapper>();

            var orders = await csv.GetRecordsAsync<CreateOrderCsvModel>().ToArrayAsync();

            _logger.LogInformation($"Successfully processed {orders.Length} orders");

            return orders;
        }
        catch (HeaderValidationException ex)
        {
            _logger.LogError(ex, "CSV header validation failed");
            throw;
        }
        catch (CsvHelperException ex)
        {
            _logger.LogError(ex, "Error parsing CSV");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");
            throw new InvalidOperationException();
        }
    }

    private sealed class InternalOrderCsvMapper : ClassMap<CreateOrderCsvModel>
    {
        public InternalOrderCsvMapper()
        {
            Map(m => m.CustomerCode).Name("Codigo cliente");
            Map(m => m.ProductCode).Name("Codigo do produto");
            Map(m => m.Quantity).Name("Quantidade");
            Map(m => m.Price).Name("Preco");
            // If your CSV has different column names:
            // Map(m => m.CustomerCode).Name("CustCode");
            // Map(m => m.Price).Name("UnitPrice");
        }
    }
}
