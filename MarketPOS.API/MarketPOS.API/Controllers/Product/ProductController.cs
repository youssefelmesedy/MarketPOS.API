using MarketPOS.API.Middlewares.FeaturesFunction;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.API.Controllers.Product;
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<ProductController> _localizar;
    public ProductController(IMediator mediator, IStringLocalizer<ProductController> localizar = null!)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _localizar = localizar ?? throw new ArgumentNullException(nameof(mediator));
    }

    // ✅ Pagination
    [HttpGet("page/")]
    [ProducesResponseType(typeof(ResultDto<PagedResultDto<ProductDetailsDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPage(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] List<ProductInclude>? includes = null,
        [FromQuery] bool SofteDelete = false)
    {
        var result = await _mediator.Send(new GetPagedProductQuery(pageIndex, pageSize, includes, SofteDelete));

        return HelperMethod.HandleResult(result, _localizar);
    }

    // ✅ Get All
    [HttpGet("GetAll/")]
    [ProducesResponseType(typeof(ResultDto<IEnumerable<SomeFeaturesProductDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] bool SofteDelete)
    {
        var result = await _mediator.Send(new GetAllProductsQuery(SofteDelete));

        return HelperMethod.HandleResult(result, _localizar);
    }

    // ✅ Get By ID
    [HttpGet("GetById/{id}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ResultDto<IEnumerable<ProductDetailsDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] bool SofteDelete)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id, SofteDelete));

        return HelperMethod.HandleResult(result, _localizar); 
    }

    // ✅ Get By ID
    [HttpGet("GetByName/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "name", ParameterValidationType.NonEmptyString })]
    [ProducesResponseType(typeof(ResultDto<IEnumerable<SomeFeaturesProductDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByName([FromQuery] string name, [FromQuery] bool includeSofteDelete)
    {
        var result = await _mediator.Send(new GetByNameProductQuery(name, includeSofteDelete));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpGet("GetWithCategoryId/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "categoryId", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ResultDto<PagedResultDto<ProductDetailsDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> WithCategoryId([FromQuery] Guid categoryId, [FromQuery] bool includeSofteDelete, [FromQuery] int pageSize = 0, [FromQuery] int pageIndex = 0)
    {
        var result = await _mediator.Send(new GetProductWithCategoryIdQuery(categoryId, includeSofteDelete, pageSize, pageIndex));

        return HelperMethod.HandleResult(result, _localizar);
    }

    // ✅ Create Product
    [HttpPost("Create/")]
    [ProducesResponseType(typeof(ResultDto<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<ProductDetailsDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResultDto<ConflictObjectResult>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var result = await _mediator.Send(new CreateProductCommand(dto));

        return await HelperMethod.HandleCreatedResult(
            result,
            nameof(GetById),
            async (id) => await _mediator.Send(new GetProductByIdQuery(id, false)),
            _localizar
        );
    }

    // ✅ Update Product
    [HttpPut("Update/{id}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ResultDto<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<ProductDetailsDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResultDto<ConflictObjectResult>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        if (id != dto.Id)
            return BadRequest(new ResultDto<object>
            {
                IsSuccess = false,
                Message = _localizar["IdMismatch"]
            });

        var result = await _mediator.Send(new UpdateProductCommand(dto));

        return await HelperMethod.ProcessResultAsync(
             result,
             async (id) => await _mediator.Send(new GetProductByIdQuery(id, false)),
             _localizar
         );
    }

    // ✅ Delete Product
    [HttpDelete("Delete/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ResultDto<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<NotFoundObjectResult>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResultDto<BadRequestObjectResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultDto<ExtendedProblemDetails>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));

        return HelperMethod.HandleResult(result, _localizar);
    }

    // ✅ Soft Delete Product
    [HttpPatch("SofteDelete/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ResultDto<SofteDeleteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<NotFoundObjectResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultDto<BadRequestObjectResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultDto<ExtendedProblemDetails>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SofteDelete([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new SofteDeleteProductQuery(id, true));

        return HelperMethod.HandleResult(result, _localizar);
    }

    // ✅ Restore Product
    [HttpPatch("Restore/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ResultDto<RestorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<NotFoundObjectResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultDto<BadRequestObjectResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultDto<ExtendedProblemDetails>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Restore([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new RestoreProductQuery(id));

        return HelperMethod.HandleResult(result, _localizar);
    }
}
