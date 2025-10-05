using MarketPOS.API.Middlewares.FeaturesFunction;
using MarketPOS.Application.Features.CQRS.CQRSWareHouse.Command;
using MarketPOS.Application.Features.CQRS.CQRSWareHouse.Query;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;
using MarketPOS.Shared.DTOs.WareHouseDTO;

namespace MarketPOS.API.Controllers.WareHouse
{
    [Route("api/[controller]")]
    [ApiController]
    public class WareHousesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<WareHousesController> _localizar;
        public WareHousesController(IMediator mediator, IStringLocalizer<WareHousesController> localizar = null!)
        {
            _mediator = mediator;
            _localizar = localizar;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(ResultDto<WareHouseDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<NotFoundResult>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<ExtendedProblemDetails>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] bool SoftDeleted)
        {
            var result = await _mediator.Send(new GetAllWareHouseQuery(SoftDeleted));

            return HelperMethod.HandleResult(result, _localizar);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ResultDto<WareHouseDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<NotFoundResult>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
        public async Task<IActionResult> GetById(Guid Id, [FromQuery] bool SoftDeleted)
        {
            var result = await _mediator.Send(new GetByIdWareHouseQuery(Id, SoftDeleted));
            return HelperMethod.HandleResult(result, _localizar);
        }

        [HttpPost()]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ConflictResult>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] WareHouseCreateDto dto)
        {
            var result = await _mediator.Send(new CreateWareHouseCommand(dto));
            return await HelperMethod.HandleCreatedResult(
                result,
                nameof(GetById),
                async (id) => await _mediator.Send(new GetByIdWareHouseQuery(id, false)),
                _localizar
            );
        }

        [HttpPut("{id:guid}")]
        [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ConflictResult>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] WareHouseUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new ResultDto<object>
                {
                    IsSuccess = false,
                    Message = _localizar["IdMismatch"]
                });
            }

            var result = await _mediator.Send(new UpdateWareHouseCommand(dto));
            return await HelperMethod.ProcessResultAsync(
                result,
                async (id) => await _mediator.Send(new GetByIdWareHouseQuery(id, false)),
                _localizar
            );
        }


        [HttpPatch("IsDelete/{id:guid}")]
        [ProducesResponseType(typeof(ResultDto<SofteDeleteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<NotFoundResult>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<ExtendedProblemDetails>), StatusCodes.Status500InternalServerError)]
        [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
        public async Task<IActionResult> SofteDelete(Guid Id)
        {
            var result = await _mediator.Send(new SofteDeleteWareHouseQuery(Id));
            return HelperMethod.HandleResult(result, _localizar);
        }

        [HttpPatch("Restor/{id:guid}")]
        [ProducesResponseType(typeof(ResultDto<RestorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<NotFoundResult>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResultDto<BadRequestResult>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<ExtendedProblemDetails>), StatusCodes.Status500InternalServerError)]
        [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
        public async Task<IActionResult> Restored([FromQuery] Guid Id)
        {
            var result = await _mediator.Send(new RestorWareHouseQuery(Id));
            return HelperMethod.HandleResult(result, _localizar);
        }
    }
}
