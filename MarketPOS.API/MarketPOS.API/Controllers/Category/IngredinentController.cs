using MarketPOS.API.Middlewares.FuatuersFunction;
using MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Command;
using MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Query;
using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.API.Controllers.Category;

[Route("api/[controller]")]
[ApiController]
public class IngredinentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<IngredinentController> _localizar;
    public IngredinentController(IMediator mediator, IStringLocalizer<IngredinentController> localizar = null!)
    {
        _mediator = mediator;
        _localizar = localizar;
    }

    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] bool SoftDeleted)
    {
        var result = await _mediator.Send(new GetAllActiveIngredinentQuery(SoftDeleted));
        if(result.Errors is not null)
            return ErrorFunction.BadRequest(result.IsSuccess, result.Message, result.Errors);

        if (result.Data is null || !result.Data.Any())
            return ErrorFunction.NotFound(result.IsSuccess, result.Message, result.Errors);

        return Ok(result);
    }

    // GET api/<IngredinentController>/5
    [HttpGet("GetById/{id}")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] bool Softdeleted)
    {
        var result = await _mediator.Send(new GetByIdInegredinentQuery(id, Softdeleted));
        if (result.Data is null)
            return ErrorFunction.NotFound(false, result.Message, result.Errors);

        return Ok(result);
    }

    [HttpGet("GetByName/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Name", ParameterValidationType.NonEmptyString})]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByName(string Name, [FromQuery] bool Softdeleted)
    {
        var result = await _mediator.Send(new GetIngredinentByNameQuery(Name, Softdeleted));
        if (result.Errors is not null)
            return ErrorFunction.BadRequest(false, result.Message, result.Errors);
        else if(result.Data is null)
            return ErrorFunction.NotFound(false, result.Message, result.Errors);

            return Ok(result);
    }

    // POST api/<IngredinentController>
    [HttpPost("Create/")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CommandActiveIngredinentsDTO dto)
    {

        var result = await _mediator.Send(new CreateActivIngredinentCommand(dto));
        if(result.Errors is not null)
            return ErrorFunction.BadRequest(result.IsSuccess, result.Message, result.Errors);

        if (result.Message!.Equals(_localizar["DuplicateActiveIngredinentName"]))
            return ErrorFunction.ConflictRequest(result.IsSuccess, result.Message, result.Errors);

        if (result.Data == Guid.Empty)
            return ErrorFunction.BadRequest(false, result.Message, result.Errors);

        var createdItem = await _mediator.Send(new GetByIdInegredinentQuery(result.Data, false));
        return CreatedAtAction(nameof(GetById), new { id = createdItem.Data!.Id, Softdeleted = false }, createdItem);
    }

    // PUT api/<IngredinentController>/5
    [HttpPut("Update/")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    public async Task<IActionResult> Put([FromQuery]Guid id, [FromBody] CommandActiveIngredinentsDTO dto, [FromQuery] bool SofteDelete)
    {
        var result = await _mediator.Send(new UpdateIngredinentCommand(id, dto, SofteDelete));

        if (result.Errors is not null)
            return ErrorFunction.BadRequest(result.IsSuccess, result.Message, result.Errors);

        if (result.Message!.Equals(_localizar["DuplicateActiveIngredinentName"]))
            return ErrorFunction.ConflictRequest(result.IsSuccess, result.Message, result.Errors);

        if (result.Data == Guid.Empty)
            return ErrorFunction.BadRequest(false, result.Message, result.Errors);

        return Ok(result);
    }

    [HttpPatch("SofteDelete/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SofteDelete([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new SofteDeleteIngredinentCommand(id));
        if(result.Errors is not null)
            return ErrorFunction.BadRequest(result.IsSuccess, result.Message, result.Errors);

        if (result.Data is null)
            return ErrorFunction.NotFound(false, result.Message, result.Errors);

        return Ok(result);
    }

    [HttpPatch("Restore/")]
    [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Restore([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new RestorIngredinentCommand(id));
        if(result.Errors is not null)
            return ErrorFunction.BadRequest(result.IsSuccess, result.Message, result.Errors);

        if (result.Data is null)
            return ErrorFunction.NotFound(false, result.Message, result.Errors);

        return Ok(result);
    }
}
