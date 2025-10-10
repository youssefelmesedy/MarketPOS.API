using MarketPOS.API.Middlewares.FeaturesFunction;
using MarketPOS.Application.Features.CQRS.CQRSAuth.Command;
using MarketPOS.Shared.DTOs.Authentication;
using MarketPOS.Shared.DTOs.AuthenticationDTO;
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
    public async Task<IActionResult> Register([FromForm] RegisterationDto registerDto, PersonFolderNameImages folderName)
    {
        var rehreshtoken = Request.Cookies["refreshToken"];

        if (registerDto == null)
            return ErrorFunction.BadRequest(false, "Invalid registration data.");

        var result = await _mediator.Send(new RegisterCommand(registerDto, folderName.ToString()));
        if (!result.IsSuccess)
            return ErrorFunction.BadRequest(result.IsSuccess, result.Message, result.Errors);

        SetRefreshTokenInCookie(result.Data!.RefreshToken, result.Data.ExpiresAt!.Value);

        return HelperMethod.HandleResult(result, _localizer);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (loginDto == null)
            return ErrorFunction.BadRequest(false,"Invalid login data.");

        var rehreshToken = Request.Cookies["refreshToken"];

        var result = await _mediator.Send(new LoginCommand(loginDto));
        if (!result.IsSuccess)
            return ErrorFunction.BadRequest(result.IsSuccess, result.Message, result.Errors);

        SetRefreshTokenInCookie(result.Data!.RefreshToken, result.Data.ExpiredAt ?? default);

        return HelperMethod.HandleResult(result, _localizer);
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var result = await _mediator.Send(new RefreshTokenCommand(refreshToken));
        if (!result.IsSuccess)
            return ErrorFunction.NotFound(result.IsSuccess, result.Message, result.Errors);

        SetRefreshTokenInCookie(result.Data!.RefreshToken, result.Data.ExpiredAt ?? default);

        return HelperMethod.HandleResult(result, _localizer);
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Cookies["refreshtoken"];

        if (token == null)
            return ErrorFunction.BadRequest(false, "Can't Insert Token Empty..!");

        var logout = await _mediator.Send(new LogoutCommand(token));
        if (!logout.IsSuccess)
            return ErrorFunction.NotFound(logout.IsSuccess, logout.Message, logout.Errors);

        Response.Cookies.Delete("refreshToken");
        return HelperMethod.HandleResult(logout, _localizer);
    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDto dto)
    {
        var command = new RequestPasswordResetCommand(dto);
        if (command.Dto.Email == null)
            return ErrorFunction.BadRequest(false, "Can't Send Empty Email");

        var result = await _mediator.Send(command);
        if (result is null || !result.IsSuccess)
            return ErrorFunction.NotFound(result!.IsSuccess, result.Message, result.Errors);

        return HelperMethod.HandleResult(result, _localizer);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var command = new ResetPasswordCommand(dto);

        if (command.Dto.Token == null || command.Dto.NewPassword == null)
            return ErrorFunction.BadRequest(false, "Token or New Password cannot be null.");

        var result = await _mediator.Send(command);

        if (result is null || !result.IsSuccess)
            return ErrorFunction.NotFound(result!.IsSuccess, result.Message, result.Errors);

        return HelperMethod.HandleResult(result, _localizer);
    }

    [HttpPost("UploadImage")]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageDto dto, PersonFolderNameImages folderName, CancellationToken cancellationToken)
    {
        if (dto == null || dto.File!.Length == 0)
            return ErrorFunction.BadRequest(false,"No file uploaded.");

        // Call your service to handle the file upload
        var result = await _mediator.Send(new UplodeFileCommand(dto.UserId, dto.PersoneFullName!, dto.File, folderName.ToString()), cancellationToken);

        if (!result.IsSuccess)
            return ErrorFunction.NotFound(result.IsSuccess, result.Message, result.Errors);

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
