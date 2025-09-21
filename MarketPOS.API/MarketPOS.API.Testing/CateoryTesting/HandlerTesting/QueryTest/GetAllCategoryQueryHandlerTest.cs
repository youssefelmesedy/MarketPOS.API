using Market.Domain.Entitys.DomainCategory;

namespace MarketPOS.API.Testing.CateoryTesting.HandlerTesting.QueryTest
{
    public class GetAllCategoryQueryHandlerTest
    {
        private readonly Mock<IServiceFactory> _serviceFactoryMock = new();
        private readonly Mock<ICategoryService> _categoryServiceMock = new();
        private readonly Mock<IResultFactory<GetAllCategoryQueryHandler>> _resultFactoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IStringLocalizer<GetAllCategoryQueryHandler>> _localizerMock = new();
        private readonly Mock<ILocalizationPostProcessor> _localizationPostProcessorMock = new();

        private GetAllCategoryQueryHandler CreateHandler()
        {
            _serviceFactoryMock
                .Setup(f => f.GetService<ICategoryService>())
                .Returns(_categoryServiceMock.Object);

            return new GetAllCategoryQueryHandler(
                _serviceFactoryMock.Object,
                _resultFactoryMock.Object,
                _mapperMock.Object,
                _localizerMock.Object,
                _localizationPostProcessorMock.Object
            );
        }

        private void SetupGetAll(IEnumerable<Category> entities, List<CategoryDetalisDto> dtos)
        {
            _categoryServiceMock
                .Setup(s => s.GetAllAsync(
                    It.IsAny<bool>(),
                    It.IsAny<List<Func<IQueryable<Category>, IQueryable<Category>>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                    It.IsAny<bool>()
                ))
                .ReturnsAsync(entities);

            _mapperMock
                .Setup(m => m.Map<List<CategoryDetalisDto>>(entities))
                .Returns(dtos);

            _localizationPostProcessorMock
                .Setup(l => l.Apply(dtos))
                .Returns(dtos);

            _resultFactoryMock
                .Setup(r => r.Success(dtos, "Success"))
                .Returns(new ResultDto<List<CategoryDetalisDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = "Success"
                });
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenDataExists()
        {
            // Arrange
            var query = new GetAllCategoryQuery(false);
            var entities = new List<Category> { new Category { Name = "EntityCat", Description = "Testing Category"} };
            var dtos = new List<CategoryDetalisDto> { new CategoryDetalisDto { Name = "DtoCat" } };
            SetupGetAll(entities, dtos);

            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data!);
            Assert.Equal("DtoCat", result.Data![0].Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoData()
        {
            // Arrange
            var query = new GetAllCategoryQuery(false);
            var entities = new List<Category>();
            var dtos = new List<CategoryDetalisDto>();
            SetupGetAll(entities, dtos);

            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data!);
        }

        [Fact]
        public async Task Handle_ShouldMapEntitiesToDtos()
        {
            // Arrange
            var query = new GetAllCategoryQuery(false);
            var entities = new List<Category> { new Category { Name = "Entity", Description = "Testing Category" } };
            var dtos = new List<CategoryDetalisDto> { new CategoryDetalisDto { Name = "Mapped" } };
            SetupGetAll(entities, dtos);

            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<List<CategoryDetalisDto>>(entities), Times.Once);
            Assert.Equal("Mapped", result.Data![0].Name);
        }

        [Fact]
        public async Task Handle_ShouldApplyLocalization()
        {
            // Arrange
            var query = new GetAllCategoryQuery(false);
            var entities = new List<Category> { new Category { Name = "Entity", Description = "Testing Category" } };
            var dtos = new List<CategoryDetalisDto> { new CategoryDetalisDto { Name = "DtoOriginal" } };
            var localized = new List<CategoryDetalisDto> { new CategoryDetalisDto { Name = "Localized" } };

            _categoryServiceMock
                .Setup(s => s.GetAllAsync(
                    It.IsAny<bool>(),
                    It.IsAny<List<Func<IQueryable<Category>, IQueryable<Category>>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                    It.IsAny<bool>()
                ))
                .ReturnsAsync(entities);

            _mapperMock
                .Setup(m => m.Map<List<CategoryDetalisDto>>(entities))
                .Returns(dtos);

            _localizationPostProcessorMock
                .Setup(l => l.Apply(dtos))
                .Returns(localized);

            _resultFactoryMock
                .Setup(r => r.Success(localized, "Success"))
                .Returns(new ResultDto<List<CategoryDetalisDto>>
                {
                    IsSuccess = true,
                    Data = localized,
                    Message = "Success"
                });

            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal("Localized", result.Data![0].Name);
        }

        [Fact]
        public async Task Handle_ShouldPassSoftDeleteFlag_ToService()
        {
            // Arrange
            var query = new GetAllCategoryQuery(true);
            var entities = new List<Category>();
            var dtos = new List<CategoryDetalisDto>();
            SetupGetAll(entities, dtos);

            var handler = CreateHandler();

            // Act
            await handler.Handle(query, CancellationToken.None);

            // Assert
            _categoryServiceMock.Verify(s => s.GetAllAsync(
                It.IsAny<bool>(),
                It.IsAny<List<Func<IQueryable<Category>, IQueryable<Category>>>>(),
                true, // soft delete flag must be true
                It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<bool>()
            ), Times.Once);
        }
    }
}