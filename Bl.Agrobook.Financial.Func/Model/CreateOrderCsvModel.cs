namespace Bl.Agrobook.Financial.Func.Model;

public class CreateOrderCsvModel
{
    public int CustomerCode { get; set; }
    public int ProductCode { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}


public class CreationOrderResultCsvModel
{
    public string? Code { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
