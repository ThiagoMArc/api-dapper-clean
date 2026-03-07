using ApiDapperClean.Api.Requests;
using ApiDapperClean.Application.DTOs.Product;

namespace ApiDapperClean.Api.Mappers;

public static class ProductsMapper
{
    public static GetProductsDto ToDto(this GetProductsRequest request)
    {
        return new GetProductsDto
        {
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
