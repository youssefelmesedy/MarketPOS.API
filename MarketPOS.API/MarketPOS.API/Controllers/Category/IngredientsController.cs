using MarketPOS.API.Middlewares.FeaturesFunction;
using MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Command;
using MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Query;
using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.API.Controllers.Category;

[Route("api/[controller]")]
[ApiController]
public class IngredientsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<IngredientsController> _localizar;
    public IngredientsController(IMediator mediator, IStringLocalizer<IngredientsController> localizar)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _localizar = localizar ?? throw new ArgumentNullException(nameof(localizar));
    }

    [HttpGet()]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] bool SoftDeleted)
    {
        var result = await _mediator.Send(new GetAllActiveIngredinentQuery(SoftDeleted));

        return HelperMethod.HandleResult(result, _localizar);
    }

    // GET api/<IngredientsController>/5
    [HttpGet("{id:guid}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] bool Softdeleted)
    {
        var result = await _mediator.Send(new GetByIdInegredinentQuery(id, Softdeleted));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpGet("{name}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "GetByName", ParameterValidationType.NonEmptyString })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByName(string name, [FromQuery] bool Softdeleted)
    {
        var result = await _mediator.Send(new GetIngredinentByNameQuery(name, Softdeleted));

        return HelperMethod.HandleResult(result, _localizar);
    }

    // POST api/<IngredientsController>
    [HttpPost()]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CommandActiveIngredinentsDTO dto)
    {

        var result = await _mediator.Send(new CreateActivIngredinentCommand(dto));

        return await HelperMethod.HandleCreatedResult(
         result,
         nameof(GetById),
         async (id) => await _mediator.Send(new GetByIdInegredinentQuery(id, false)),
         _localizar
        );
    }

    // PUT api/<IngredientsController>/5
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    public async Task<IActionResult> Put(Guid id, [FromBody] CommandActiveIngredinentsDTO dto, [FromQuery] bool SofteDelete)
    {
        var result = await _mediator.Send(new UpdateIngredinentCommand(id, dto, SofteDelete));

        return await HelperMethod.ProcessResultAsync(
            result,
            async(id) => await _mediator.Send(new GetByIdInegredinentQuery(id, false)),
            _localizar);
    }

    [HttpPatch("IsDelete/{id:guid}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SofteDelete(Guid id)
    {
        var result = await _mediator.Send(new SofteDeleteIngredinentCommand(id));

        return HelperMethod.HandleResult(result, _localizar);
    }

    [HttpPatch("Restore/{id:guid}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _mediator.Send(new RestorIngredinentCommand(id));

        return HelperMethod.HandleResult(result, _localizar);
    }
}
