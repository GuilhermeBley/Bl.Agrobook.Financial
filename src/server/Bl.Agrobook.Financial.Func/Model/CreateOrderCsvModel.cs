namespace Bl.Agrobook.Financial.Func.Model;

public class CreateOrderCsvModel
{
    public string CustomerCode { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string? ObsPedido { get; set; }
}


public class CreationOrderResultCsvModel
{
    public string? Code { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
