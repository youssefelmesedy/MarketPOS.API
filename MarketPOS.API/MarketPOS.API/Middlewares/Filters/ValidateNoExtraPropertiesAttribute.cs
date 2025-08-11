using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Method)]
public class ValidateNoExtraPropertiesAttribute : ActionFilterAttribute
{
    private readonly string _paramName;
    private readonly JsonSerializerOptions _jsonOptions;

    public ValidateNoExtraPropertiesAttribute(string paramName)
    {
        _paramName = paramName;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;
        request.EnableBuffering();

        string bodyStr;
        using (var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
        {
            bodyStr = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        if (!string.IsNullOrWhiteSpace(bodyStr))
        {
            try
            {
                var jsonDict = JsonSerializer.Deserialize<Dictionary<string, object>>(bodyStr, _jsonOptions);

                if (jsonDict != null)
                {
                    var paramDescriptor = context.ActionDescriptor.Parameters
                        .FirstOrDefault(p => p.Name == _paramName);

                    if (paramDescriptor is ControllerParameterDescriptor descriptor)
                    {
                        // أول ما نلاقي خاصية زيادة نرجع BadRequest على طول
                        var extraProp = FindFirstExtraPropertyRecursive(jsonDict, descriptor.ParameterType);
                        if (extraProp != null)
                        {
                            context.ModelState.AddModelError(extraProp, $"Unexpected property: {extraProp}");
                            context.Result = new BadRequestObjectResult(context.ModelState);
                            return;
                        }
                    }
                }
            }
            catch (JsonException)
            {
                context.Result = new BadRequestObjectResult("Invalid JSON format.");
                return;
            }
        }

        await next();
    }

    private string FindFirstExtraPropertyRecursive(Dictionary<string, object> jsonDict, Type modelType)
    {
        var modelProps = modelType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new
            {
                Name = p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? p.Name,
                PropertyType = p.PropertyType
            })
            .ToList();

        var allowedNames = modelProps.Select(mp => mp.Name).ToList();

        // لو فيه أي مفتاح مش موجود في الموديل → رجع اسمه فورًا
        var extra = jsonDict.Keys.FirstOrDefault(k => !allowedNames.Contains(k, StringComparer.OrdinalIgnoreCase));
        if (extra != null)
            return extra;

        // ندخل على الخصائص المتداخلة
        foreach (var modelProp in modelProps)
        {
            if (!jsonDict.TryGetValue(modelProp.Name, out var value) || value == null)
                continue;

            if (value is JsonElement elem)
            {
                if (elem.ValueKind == JsonValueKind.Object)
                {
                    var nestedDict = JsonSerializer.Deserialize<Dictionary<string, object>>(elem.GetRawText(), _jsonOptions);
                    if (nestedDict != null)
                    {
                        var nestedExtra = FindFirstExtraPropertyRecursive(nestedDict, modelProp.PropertyType);
                        if (nestedExtra != null)
                            return nestedExtra;
                    }
                }
                else if (elem.ValueKind == JsonValueKind.Array)
                {
                    var elemType = modelProp.PropertyType.IsArray
                        ? modelProp.PropertyType.GetElementType()
                        : (modelProp.PropertyType.IsGenericType
                            ? modelProp.PropertyType.GetGenericArguments().FirstOrDefault()
                            : null);

                    if (elemType != null && !elemType.IsPrimitive && elemType != typeof(string))
                    {
                        foreach (var item in elem.EnumerateArray())
                        {
                            if (item.ValueKind == JsonValueKind.Object)
                            {
                                var nestedDict = JsonSerializer.Deserialize<Dictionary<string, object>>(item.GetRawText(), _jsonOptions);
                                if (nestedDict != null)
                                {
                                    var nestedExtra = FindFirstExtraPropertyRecursive(nestedDict, elemType);
                                    if (nestedExtra != null)
                                        return nestedExtra;
                                }
                            }
                        }
                    }
                }
            }
        }

        return null;
    }
}
