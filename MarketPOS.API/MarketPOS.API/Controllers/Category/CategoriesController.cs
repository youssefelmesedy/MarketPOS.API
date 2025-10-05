using MarketPOS.API.ExtensionsFiltreingAndMiddlewares.ExtensionAttribute;
using MarketPOS.API.Middlewares.FeaturesFunction;
using MarketPOS.Shared.Constants;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;
using Microsoft.AspNetCore.Authorization;

namespace MarketPOS.API.Controllers.Category;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<CategoriesController> _localizar;
    public CategoriesController(IMediator mediator, IStringLocalizer<CategoriesController> localizar = null!)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _localizar = localizar ?? throw new ArgumentNullException(nameof(localizar));
    }

    [HttpGet()]
    [ProducesResponseType(typeof(ResultDto<CategoryDetalisDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultDto<NotFoundResult>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] bool SoftDeleted)
    {
        var result = await _mediator.Send(new GetAllCategoryQuery(SoftDeleted));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpGet("{id:guid}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ResultDto<CategoryDetalisDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultDto<NotFoundResult>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] bool Softdeleted)
    {
        var result = await _mediator.Send(new GetByIdCategoryQuery(id, Softdeleted));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpGet("{name}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "name", ParameterValidationType.NonEmptyString })]
    [ProducesResponseType(typeof(ResultDto<CategoryDetalisDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultDto<NotFoundResult>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByName(string name, [FromQuery] bool IncludsofteDelete)
    {
        var result = await _mediator.Send(new GetCategoryName(name, IncludsofteDelete));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpPost()]
    [ProducesResponseType(typeof(ResultDto<CreateCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
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

    [HttpPut("{id:guid}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ResultDto<CategoryUpdateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
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

    [HttpDelete("{id:guid}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ResultDto<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand(id));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpPatch("IsDelete/{id:guid}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ResultDto<SofteDeleteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<NotFoundResult>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SofteDelete(Guid id)
    {
        var result = await _mediator.Send(new SofteCategoryDeletedQuery(id));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpPatch("Restore/{id:guid}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ResultDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultDto<NotFoundResult>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _mediator.Send(new RestoreCategoryQuery(id));

        return HelperMethod.HandleResult(result, _localizar);
    }
}
