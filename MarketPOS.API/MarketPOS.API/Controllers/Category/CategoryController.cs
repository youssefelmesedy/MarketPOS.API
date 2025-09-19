using MarketPOS.API.Middlewares.FeaturesFunction;

namespace MarketPOS.API.Controllers.Category;
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<CategoryController> _localizar;
    public CategoryController(IMediator mediator, IStringLocalizer<CategoryController> localizar = null!)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _localizar = localizar ?? throw new ArgumentNullException(nameof(localizar));
    }

    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] bool SoftDeleted)
    {
        var result = await _mediator.Send(new GetAllCategoryQuery(SoftDeleted));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpGet("GetById/{id}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] bool Softdeleted)
    {
        var result = await _mediator.Send(new GetByIdCategoryQuery(id, Softdeleted));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpGet("GetByName/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "name", ParameterValidationType.NonEmptyString })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByName([FromQuery] string name, [FromQuery] bool IncludsofteDelete)
    {
        var result = await _mediator.Send(new GetCategoryName(name, IncludsofteDelete));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpPost("Create/")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
    {
        var result = await _mediator.Send(new CreateCategoryCommand(dto));

        return await HelperMethod.HandleCreatedResult
            (
            result,
            nameof(GetById),
            async (id) => await _mediator.Send(new GetByIdCategoryQuery(id, false)),
            _localizar
            );
    }

    [HttpPut("Update/{id}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CategoryUpdateDto dto, [FromQuery] bool SofteDelete)
    {
        if (id != dto.Id)
            return ErrorFunction.BadRequest(false, _localizar["IdMismatch"], null);

        var result = await _mediator.Send(new UpdateCategoryCommand(dto, SofteDelete));

        return await HelperMethod.ProcessResultAsync(
            result,
            async (id) => await _mediator.Send(new GetByIdCategoryQuery(id, false)),
            _localizar
        );
    }

    [HttpDelete("Delete/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand(id));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpPatch("SofteDelete/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SofteDelete([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new SofteCategoryDeletedQuery(id));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpPatch("Restore/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Restore([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new RestoreCategoryQuery(id));

        return HelperMethod.HandleResult(result, _localizar);
    }
}
