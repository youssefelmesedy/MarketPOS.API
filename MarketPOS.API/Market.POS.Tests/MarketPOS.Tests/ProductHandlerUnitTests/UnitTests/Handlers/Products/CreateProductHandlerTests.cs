using AutoMapper;
using FluentAssertions;
using Market.Domain.Entitys.DomainCategory;
using MarketPOS.Application.Common.Exceptions;
using MarketPOS.Application.Features.CQRS.CQRSProduct.Command;
using MarketPOS.Application.Features.CQRS.CQRSProduct.Command.HandlerCommand;
using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Design.FactoryResult;
using MarketPOS.Design.FactoryServices;
using MarketPOS.Shared.DTOs.ProductDto;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

public class CreateProductHandlerTests
{
    [Fact]
    public async Task Should_Return_Error_When_Category_Not_Found()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "TestProduct",
            Barcode = "123456",
            CategoryId = Guid.NewGuid(),
            SalePrice = 90,
            PurchasePrice = 90,
            LargeUnitName = "Carton",
            MediumUnitName = "Box",
            SmallUnitName = "Piece",
            MediumPerLarge = 10,
            SmallPerMedium = 10
        };

        var categoryServiceMock = new Mock<ICategoryService>();
        categoryServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), false, null, false, false))
                           .ReturnsAsync((Category?)null);

        var serviceFactoryMock = new Mock<IServiceFactory>();
        serviceFactoryMock.Setup(f => f.GetService<ICategoryService>()).Returns(categoryServiceMock.Object);

        var resultFactory = new ResultFactory<CreateProductCommandHandler>();
        var mapperMock = new Mock<IMapper>();
        var localizerMock = new Mock<IStringLocalizer<CreateProductCommandHandler>>();

        var handler = new CreateProductCommandHandler(serviceFactoryMock.Object, resultFactory, mapperMock.Object, localizerMock.Object);

        var command = new CreateProductCommand(dto);

        // Act
        Func<Task> act = async () => { await handler.Handle(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
