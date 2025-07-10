using AutoMapper;
using MarketPOS.Application.Common.Exceptions;
using MarketPOS.Application.Services.Interfaces;
using MarketPOS.Design.FactoryServices;
using MarketPOS.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Localization;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Command.CommandHandler;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ResultDto<Guid>>
{
    private readonly IServiceFactory _serviceFactory;
    private readonly IStringLocalizer<DeleteCategoryCommandHandler> _localizer; // Fix: Declare the field properly

    public DeleteCategoryCommandHandler(IServiceFactory serviceFactory, IStringLocalizer<DeleteCategoryCommandHandler> serviceLocalizer)
    {
        _serviceFactory = serviceFactory;
        _localizer = serviceLocalizer; // Fix: Initialize the field in the constructor
    }

    public async Task<ResultDto<Guid>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var service = _serviceFactory.GetService<ICategoryService>();

        var category = await service.GetByIdAsync(request.Id);
        if  (category == null) 
            throw new NotFoundException($"Not Found Category with Id: {request.Id}");

         await service.RemoveAsync(category);

        return new ResultDto<Guid>
        {
            IsSuccess = true,
            Message = _localizer["Deleted"].Value, // Use the localizer to get the message
            Data = request.Id
        };
    }
}
