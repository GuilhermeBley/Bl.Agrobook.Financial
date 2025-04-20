namespace Bl.Agrobook.Financial.Func.Model;

public class CreateOrderCsvModel
{
    public int CustomerCode { get; set; }
    public int ProductCode { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
