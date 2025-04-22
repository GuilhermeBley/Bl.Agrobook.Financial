using Azure;
using Azure.Data.Tables;

namespace Bl.Agrobook.Financial.Func.Model;

public class AzureTableAuthModel : ITableEntity
{
    public string? Token { get; set; }
    public string PartitionKey { get; set; } = "token";
    public string RowKey { get; set; } = "token";
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
