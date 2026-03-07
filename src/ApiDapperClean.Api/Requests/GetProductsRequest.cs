using System.Text.Json.Serialization;

namespace ApiDapperClean.Api.Requests;

public class GetProductsRequest
{
    [JsonPropertyName("page")]
    public int Page { get; set; } = 1;
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 10;
}
