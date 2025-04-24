using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bl.Agrobook.Financial.Func.Model
{
    public class PagenableViewModel
    {
        [JsonPropertyName("pageNumber")]
        public int? PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int? PageSize { get; set; }

        [JsonPropertyName("sort")]
        public Sort Sort { get; set; } = new();

        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        [JsonPropertyName("paged")]
        public bool? Paged { get; set; }

        [JsonPropertyName("unpaged")]
        public bool? Unpaged { get; set; }
    }

    public class Sort
    {
        [JsonPropertyName("orders")]
        public List<Order> Orders { get; set; } = new();

        [JsonPropertyName("sorted")]
        public bool? Sorted { get; set; }

        [JsonPropertyName("empty")]
        public bool? Empty { get; set; }

        [JsonPropertyName("unsorted")]
        public bool? Unsorted { get; set; }
    }

    public class Order
    {
        [JsonPropertyName("direction")]
        public string? Direction { get; set; }

        [JsonPropertyName("property")]
        public string? Property { get; set; }

        [JsonPropertyName("ignoreCase")]
        public bool? IgnoreCase { get; set; }

        [JsonPropertyName("nullHandling")]
        public string? NullHandling { get; set; }

        [JsonPropertyName("ascending")]
        public bool? Ascending { get; set; }

        [JsonPropertyName("descending")]
        public bool? Descending { get; set; }
    }
}
