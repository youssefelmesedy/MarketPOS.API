using NJsonSchema;
namespace MarketPOS.API.Extensions.ExtensionSwgger;
public class AcceptLanguageHeaderProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        var schema = new JsonSchema
        {
            Type = JsonObjectType.String,
            Enumeration = { "ar-EG", "en-US" }, // ✅ Add allowed values here
        };

        context.OperationDescription.Operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Accept-Language",
            Kind = OpenApiParameterKind.Header,
            Description = "Preferred language for localization",
            Schema = schema,
            IsRequired = false
        });

        return true;
    }
}
