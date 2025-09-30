using MarketPOS.API.Middlewares.FeaturesFunction;
using MarketPOS.Application.Features.CQRS.CQRSAuth.Command;
using MarketPOS.Shared.DTOs.Authentication;
using MarketPOS.Shared.Eunms.EunmPersonFolderNameImage;

namespace MarketPOS.API.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<AuthsController> _localizer;
    public AuthsController(IMediator mediator, IStringLocalizer<AuthsController> localizer)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] RegisterDto registerDto, PersonFolderNameImages folderName)
    {
        var rehreshtoken = Request.Cookies["refreshToken"];

        if (registerDto == null)
            return BadRequest("Invalid registration data.");

        var result = await _mediator.Send(new RegisterCommand(registerDto, folderName.ToString()));
        if (!result.IsSuccess)
            return BadRequest(result.Message);

        SetRefreshTokenInCookie(result.Data!.RefreshToken, result.Data.ExpiresAt!.Value);

        return HelperMethod.HandleResult(result, _localizer);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (loginDto == null)
            return BadRequest("Invalid login data.");

        var rehreshToken = Request.Cookies["refreshToken"];

        var result = await _mediator.Send(new LoginCommand(loginDto));
        if (!result.IsSuccess)

            return BadRequest(result.Message);

        SetRefreshTokenInCookie(result.Data!.RefreshToken, result.Data.ExpiredAt ?? default);

        return HelperMethod.HandleResult(result, _localizer);
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var result = await _mediator.Send(new RefreshTokenCommand(refreshToken));
        if (!result.IsSuccess)
            return BadRequest(result.Message);

        SetRefreshTokenInCookie(result.Data!.RefreshToken, result.Data.ExpiredAt ?? default);

        return HelperMethod.HandleResult(result, _localizer);
    }

    [HttpPost("UploadImage")]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageDto dto, PersonFolderNameImages folderName, CancellationToken cancellationToken)
    {
        if (dto == null || dto.File!.Length == 0)
            return BadRequest("No file uploaded.");

        // Call your service to handle the file upload
        var result = await _mediator.Send(new UplodeFileCommand(dto.UserId, dto.PersoneFullName!, dto.File, folderName.ToString()), cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return HelperMethod.HandleResult(result, _localizer);
    }

    private void SetRefreshTokenInCookie(string token, DateTime expiresAt)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expiresAt.ToLocalTime(),
            SameSite = SameSiteMode.Strict,
            Secure = true // Ensure the cookie is only sent over HTTPS
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
}
