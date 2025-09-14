using MarketPOS.Application.Services.InterfacesServices;

namespace MarketPOS.Application.Common.HandlerBehaviors;
public abstract class BaseHandler<THandler>
{
    protected readonly IAggregateService _productServices;
    protected readonly IServiceFactory _servicesFactory;
    protected readonly IResultFactory<THandler> _resultFactory;
    protected readonly IMapper? _mapper;
    protected readonly ILogger<THandler>? _logger;
    protected readonly IStringLocalizer<THandler>? _localizer;
    protected readonly ILocalizationPostProcessor _localizationPostProcessor;

    protected BaseHandler(
        IServiceFactory services = null!,
        IResultFactory<THandler> resultFactory = null!,
        IMapper? mapper = null,
        ILogger<THandler>? logger = null,
        IStringLocalizer<THandler>? localizer = null,
        ILocalizationPostProcessor localizationPostProcessor = null!,
        IAggregateService productServices = null!)
    {
        _servicesFactory = services;
        _resultFactory = resultFactory;
        _mapper = mapper;
        _logger = logger;
        _localizer = localizer;
        _localizationPostProcessor = localizationPostProcessor;
        _productServices = productServices;
    }

    protected ResultDto<T> Success<T>(T data, string key)
        => _resultFactory.Success(data, key);

    protected ResultDto<T> Fail<T>(string key)
        => _resultFactory.Fail<T>(key);
}

