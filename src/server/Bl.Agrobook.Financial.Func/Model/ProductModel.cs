namespace Bl.Agrobook.Financial.Func.Model;

public class ProductModel
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? AvailableQuantity { get; set; }
    public decimal? Price { get; set; }
    public string? ImgUrl { get; set; }
}
