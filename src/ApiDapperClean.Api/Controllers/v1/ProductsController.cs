using ApiDapperClean.Application.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using ApiDapperClean.Api.Requests;
using ApiDapperClean.Api.Mappers;
using ApiDapperClean.Application.Services.v1.Products.GetById;
using ApiDapperClean.Application.Services.v1.Products.GetProducts;
using ApiDapperClean.Application.Services.v1.Products.Create;
using ApiDapperClean.Application.Services.v1.Products.Update;
using ApiDapperClean.Application.Services.v1.Products.Delete;
using ApiDapperClean.Domain.Results;

namespace ApiDapperClean.Api.Controllers.v1;

/// <summary>
/// Controller para gerenciamento de produtos
/// </summary>
[ApiController]
[Route("api/v1/products")]
[Produces("application/json")]
public class ProductsController : BaseController
{
    private readonly IGetProductsService _getProductsService;
    private readonly IGetProductByIdService _getByIdService;
    private readonly ICreateProductService _createService;
    private readonly IUpdateProductService _updateService;
    private readonly IDeleteProductService _deleteService;

    public ProductsController(IGetProductsService getProductsService,
                             IGetProductByIdService getByIdService,
                             ICreateProductService createService,
                             IUpdateProductService updateService,
                             IDeleteProductService deleteService)
    {
        _getProductsService = getProductsService;
        _getByIdService = getByIdService;
        _createService = createService;
        _updateService = updateService;
        _deleteService = deleteService;
    }

    /// <summary>
    /// Obtém um produto pelo ID
    /// </summary>
    /// <param name="id">ID do produto</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do produto</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Result<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        Result<ProductDto> result = await _getByIdService.GetByIdAsync(id, cancellationToken);

        return GenerateResponse(result);
    }

    /// <summary>
    /// Obtém todos os produtos
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de produtos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetProductsRequest request, CancellationToken cancellationToken)
    {
        Result<PagedDataResult<ProductDto>> result = await _getProductsService.GetPagedAsync(request.ToDto(), cancellationToken);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Cria um novo produto
    /// </summary>
    /// <param name="dto">Dados do produto</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do produto criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Result<ProductDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateProductDto dto, CancellationToken cancellationToken)
    {
        Result<ProductDto> result = await _createService.CreateAsync(dto, cancellationToken);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Atualiza um produto existente
    /// </summary>
    /// <param name="id">ID do produto</param>
    /// <param name="dto">Dados atualizados</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do produto atualizado</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Result<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, UpdateProductDto dto, CancellationToken cancellationToken)
    {
        Result<ProductDto> result = await _updateService.UpdateAsync(id, dto, cancellationToken);
        return GenerateResponse(result);
    }

    /// <summary>
    /// Deleta um produto
    /// </summary>
    /// <param name="id">ID do produto</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        Result<bool?> result = await _deleteService.DeleteAsync(id, cancellationToken);
        return GenerateResponse(result);
    }
}
