
using MarketPOS.Application.Services.InterfacesServices.FileStorage;
using MarketPOS.Shared.DTOs.Authentication;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command.CommandHandler;
public class UplodeFileCommandHandler : BaseHandler<UplodeFileCommandHandler>,
    IRequestHandler<UplodeFileCommand, ResultDto<string>>
{
    public UplodeFileCommandHandler(
        IServiceFactory serviceFactory,
        IResultFactory<UplodeFileCommandHandler> resultFactory) 
        : base(serviceFactory, resultFactory)
    {
    }
    public async Task<ResultDto<string>> Handle(UplodeFileCommand request, CancellationToken cancellationToken)
    {
        var fileService = _servicesFactory.GetService<IFileService>();
        if (request.File is null)
            throw new ArgumentNullException(nameof(request.File));

        var result = await fileService.SaveUserImageAsync(request.UserId, request.UserName, request.File, request.FolderName, cancellationToken);
        if (string.IsNullOrEmpty(result))
            return _resultFactory.Fail<string>($"Message: Can't Uplode File Empty {result}");

        return _resultFactory.Success(result, "File uploaded successfully");
    }
}
