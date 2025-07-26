namespace MarketPOS.API.Controllers.Product;
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
    public async Task<IActionResult> Name([FromQuery] string name, [FromQuery] bool includeSofteDelete)
    {
        var result = await _mediator.Send(new GetByNameProductQuery(name, includeSofteDelete));

        return Ok(result);
    }

    [HttpGet("GetWithCategoryId/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "categoryId", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> WithCategoryId([FromQuery] Guid categoryId, [FromQuery] bool includeSofteDelete, [FromQuery] int pageSize = 0, [FromQuery] int pageIndex = 0 )
    {
        var result = await _mediator.Send(new GetProductWithCategoryIdQuery(categoryId, includeSofteDelete, pageSize, pageIndex));

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
