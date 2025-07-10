namespace MarketPOS.API.Middlewares.Filters;
public class ValidateParameterAttribute : ActionFilterAttribute
{   
    private readonly string _paramName;
    private readonly ParameterValidationType _validationType;
    private readonly Type? _enumType;

    public ValidateParameterAttribute(string paramName, ParameterValidationType validationType, Type? enumType = null)
    {
        _paramName = paramName;
        _validationType = validationType;
        _enumType = enumType;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<ValidateParameterAttribute>>();

        if (!context.ActionArguments.TryGetValue(_paramName, out var value) || value == null)
        {
            context.Result = new BadRequestObjectResult(new ExtendedProblemDetails
            {
                Title = localizer["ValidationFailed"],
                Detail = string.Format(localizer["MissingParameter"], _paramName),
                Status = 400,
                ErrorCode = "MissingParam"
            });
            return;
        }

        var isValid = _validationType switch
        {
            ParameterValidationType.Guid => Guid.TryParse(value.ToString(), out _),
            ParameterValidationType.PositiveInt => int.TryParse(value.ToString(), out var i) && i > 0,
            ParameterValidationType.NonEmptyString => !string.IsNullOrWhiteSpace(value.ToString()),
            ParameterValidationType.Enum => _enumType != null && Enum.IsDefined(_enumType, value),
            _ => true
        };

        if (!isValid)
        {
            context.Result = new BadRequestObjectResult(new ExtendedProblemDetails
            {
                Title = localizer["ValidationFailed"],
                Detail = string.Format(localizer["InvalidParameterFormat"], _paramName),
                Status = 400,
                ErrorCode = $"Invalid_{_validationType}"
            });
        }
    }
}

