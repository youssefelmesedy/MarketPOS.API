namespace MarketPOS.API;
public class ContactDocumentProcessor : IDocumentProcessor
{
    public void Process(DocumentProcessorContext context)
    {
        var document = context.Document;

        document.Info.Contact = new OpenApiContact
        {
            Name = "Y𝒐𝐔𝓢𝓢e𝓕 E𝓵𝓜𝐄𝓢𝐞𝐃𝓨",
            Url = "https://github.com/youssefelmesedy",
            Email = "yousefelmesedy6@gmail.com"
        };
    }
}
