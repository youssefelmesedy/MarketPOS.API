using AutoMapper;
using Market.Domain.Entitys.DomainCategory;
using MarketPOS.Application.Services.Interfaces;
using MarketPOS.Design.FactoryServices;
using MarketPOS.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Collections.Immutable;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Command.CommandHandler;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ResultDto<Guid>>
{
    private readonly IServiceFactory _serviceFactory;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<CreateCategoryCommandHandler> _localizer;

    public CreateCategoryCommandHandler(IServiceFactory serviceFactory, IMapper mapper, IStringLocalizer<CreateCategoryCommandHandler> localizer)
    {
        _serviceFactory = serviceFactory;
        _mapper = mapper;
        _localizer = localizer;
    }
    public async Task<ResultDto<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var serviceCategory = _serviceFactory.GetService<ICategoryService>();

        var category = _mapper.Map<Category>(request.Dto);

        var existCategoryName = await serviceCategory.FindAsync(c => c.Name.ToLower().Trim() == 
                                                                     category.Name.ToLower().Trim());
        if (existCategoryName.Any())
        {
            var existdata = existCategoryName.First().Id;

            return new ResultDto<Guid>
            {
                IsSuccess = false,
                Message = _localizer["DuplicateCategoryName"],
                Data = existdata
            };
        }

         await serviceCategory.AddAsync(category);

        return new ResultDto<Guid>
        {
            IsSuccess = true,
            Message = _localizer["Created"],
            Data = category.Id
        };
    }
}
