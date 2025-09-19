using MarketPOS.Application.Features.CQRS.CQRSWareHouse.Command;
using MarketPOS.Application.Features.CQRS.CQRSWareHouse.Query;
using MarketPOS.Shared.DTOs.WareHouseDTO;

namespace MarketPOS.API.Controllers.WareHouse
{
    [Route("api/[controller]")]
    [ApiController]
    public class WareHouseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<WareHouseController> _localizar;
        public WareHouseController(IMediator mediator, IStringLocalizer<WareHouseController> localizar = null!)
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
            var result = await _mediator.Send(new GetAllWareHouseQuery(SoftDeleted));
            return Ok(result);
        }

        [HttpGet("GetById/")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
        public async Task<IActionResult> GetById([FromQuery] Guid Id, [FromQuery] bool SoftDeleted)
        {
            var result = await _mediator.Send(new GetByIdWareHouseQuery(Id, SoftDeleted));
            return Ok(result);
        }

        [HttpPatch("SofteDelete/")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
        public async Task<IActionResult> SofteDelete([FromQuery] Guid Id)
        {
            var result = await _mediator.Send(new SofteDeleteWareHouseQuery(Id));
            return Ok(result);
        }

        [HttpPatch("RestoredBy/")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
        public async Task<IActionResult> Restored([FromQuery] Guid Id)
        {
            var result = await _mediator.Send(new RestorWareHouseQuery(Id));
            return Ok(result);
        }

        [HttpPost("Create/")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] WareHouseCreateDto dto)
        {
            var result = await _mediator.Send(new CreateWareHouseCommand(dto));

            return Ok(result);
        }

        [HttpPut("Update/")]
        [TypeFilter(typeof(ValidateParameterAttribute), Arguments = new object[] { "Id", ParameterValidationType.Guid })]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] WareHouseUpdateDto dto)
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
            return Ok(result);
        }
    }
}
