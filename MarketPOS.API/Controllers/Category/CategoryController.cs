namespace MarketPOS.API.Controllers.Category;
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<CategoryController> _localizar;
    public CategoryController(IMediator mediator, IStringLocalizer<CategoryController> localizar = null!)
    {
        _mediator = mediator;
        _localizar = localizar;
    }

    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] bool SoftDeleted)
    {
        var result = await _mediator.Send(new GetAllCategoryQuery(SoftDeleted));

        return Ok(result);
    }

    [HttpGet("GetById/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById([FromQuery]Guid id, [FromQuery] bool Softdeleted)
    {
        var result = await _mediator.Send(new GetByIdCategoryQuery(id, Softdeleted));

        return Ok(result);
    }

    [HttpGet("GetByName/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "name", ParameterValidationType.NonEmptyString})]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByName([FromQuery]string name, [FromQuery] bool IncludsofteDelete)
    {
        var result = await _mediator.Send(new GetCategoryName(name, IncludsofteDelete));

        return Ok(result);
    }

    [HttpPost("Create/")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
    {
        var result = await _mediator.Send(new CreateCategoryCommand(dto));

        return Ok(result);
    }

    [HttpPut("Update/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] CategoryUpdateDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(new ResultDto<object>
            {
                IsSuccess = false,
                Message = _localizar["IdMismatch"]
            });
        }
        var result = await _mediator.Send(new UpdateCategoryCommand(dto));

        return Ok(result);
    }

    [HttpDelete("Delete/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand(id));

        return Ok(result);
    }

    [HttpPatch("SofteDelete/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SofteDelete([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new SofteCategoryDeletedQuery(id));

        return Ok(result);
    }

    [HttpPatch("Restore/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Restore([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new RestoreCategoryQuery(id));

        return Ok(result);
    }
}
