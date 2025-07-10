using AutoMapper;
using Market.Domain.Entitys.DomainCategory;
using MarketPOS.Application.Common.Exceptions;
using MarketPOS.Application.Services.Interfaces;
using MarketPOS.Design.FactoryServices;
using MarketPOS.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Localization;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Command.CommandHandler;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ResultDto<Guid>>
{
    private readonly IServiceFactory _serviceFactory;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<UpdateCategoryCommandHandler> _localizer;

    public UpdateCategoryCommandHandler(IServiceFactory serviceFactory, IMapper mapper, IStringLocalizer<UpdateCategoryCommandHandler> localizer)
    {
        _serviceFactory = serviceFactory;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<ResultDto<Guid>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var serviceCategory = _serviceFactory.GetService<ICategoryService>();

        var esistCategory = await serviceCategory.GetByIdAsync(request.dto.Id);
        if (esistCategory == null)
            throw new NotFoundException(nameof(Category), request.dto.Id);

        var category = _mapper.Map(request.dto, esistCategory);
        var existCategoryName = await serviceCategory.FindAsync(c => c.Name.ToLower().Trim() ==
                                                                     category.Name.ToLower().Trim() &&
                                                                     c.Id != category.Id);

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

        await serviceCategory.UpdateAsync(category);

        return new ResultDto<Guid>
        {
            IsSuccess = true,
            Message = _localizer["Updated"],
            Data = category.Id
        };
    }
}
