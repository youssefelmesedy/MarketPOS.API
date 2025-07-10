using MarketPOS.API.Middlewares.Filters;
using MarketPOS.Application.Features.CQRS.CQRSProduct.Command;
using MarketPOS.Application.Features.CQRS.CQRSProduct.Query;
using MarketPOS.Shared;
using MarketPOS.Shared.DTOs;
using MarketPOS.Shared.DTOs.ProductDto;
using MarketPOS.Shared.Eunms.ProductEunms;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<ProductController> _localizar;
    public ProductController(IMediator mediator, IStringLocalizer<ProductController> localizar = null!)
    {
        _mediator = mediator;
        _localizar = localizar;
    }

    // ✅ Pagination
    [HttpGet("page/")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPage(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] List<ProductInclude>? includes = null)
    {
        var result = await _mediator.Send(new GetPagedProductQuery(pageIndex, pageSize, includes));
        return Ok(result);
    }

    // ✅ Get All
    [HttpGet("GetAll/")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] bool SofteDelete)
    {
        var result = await _mediator.Send(new GetAllProductsQuery(SofteDelete));
        return Ok(result);
    }

    // ✅ Get By ID
    [HttpGet("GetById/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById([FromQuery]Guid id, [FromQuery] bool SofteDelete)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id, SofteDelete));

        return Ok(result);
    }

    // ✅ Get By ID
    [HttpGet("GetByName/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "name", ParameterValidationType.NonEmptyString })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Name([FromQuery] string name)
    {
        var result = await _mediator.Send(new GetByNameProductQuery(name));

        return Ok(result);
    }

    // ✅ Create Product
    [HttpPost("Create/")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var result = await _mediator.Send(new CreateProductCommand(dto));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    // ✅ Update Product
    [HttpPut("Update/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] UpdateProductDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(new ResultDto<object>
            {
                IsSuccess = false,
                Message = _localizar["IdMismatch"]
            });
        }

        var result = await _mediator.Send(new UpdateProductCommand(dto));
        return Ok(result);
    }

    // ✅ Delete Product
    [HttpDelete("Delete/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));

        return Ok(result);
    }

    // ✅ Soft Delete Product
    [HttpPatch("SofteDelete/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SofteDelete([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new SofteDeleteProductQuery(id, true));

        return Ok(result);
    }

    // ✅ Restore Product
    [HttpPatch("Restore/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Restore([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new RestoreProductQuery(id));

        return Ok(result);
    }
}
