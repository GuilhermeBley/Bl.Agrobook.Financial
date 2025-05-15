using System.Text.Json.Serialization;

namespace Bl.Agrobook.Financial.Func.Model;

public class ImageViewModel
{
    public static ImageViewModel Default => new ImageViewModel()
    {
        Url = string.Empty,
        Status = 3,
        Md5 = string.Empty
    };

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("md5")]
    public string? Md5 { get; set; }
}
