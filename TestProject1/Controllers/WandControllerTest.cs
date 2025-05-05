using Moq;
using Microsoft.AspNetCore.Mvc;
using WandShop.Application.Service;
using WandShopService.Controllers;
using WandShop.Domain.Models;
using WandShop.Domain.Models.Dto;
using WandShop.Domain.Enums;

namespace WandShopService.Tests.Controllers
{
    public class WandControllerTests
    {
        private readonly Mock<IWandService> _wandServiceMock;
        private readonly WandController _controller;

        public WandControllerTests()
        {
            _wandServiceMock = new Mock<IWandService>();
            _controller = new WandController(_wandServiceMock.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnAllProducts_ReturnsTrue()
        {
            // Arrange
            var wands = new List<GetWandDto>
            {
                new GetWandDto(1, WoodType.Holly, 12.5m, WandCore.PhoenixFeather, Flexibility.Supple, 100m, false),
                new GetWandDto(2, WoodType.Yew, 13.0m, WandCore.DragonHeartstring, Flexibility.Rigid, 150m, false)
            };
            _wandServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(wands);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(wands, okResult.Value);
        }

        [Fact]
        public async Task Get_WithValidId_ReturnsProduct_ReturnsTrue()
        {
            // Arrange
            var wand = new GetWandDto(1, WoodType.Holly, 12.5m, WandCore.PhoenixFeather, Flexibility.Supple, 100m, false);
            _wandServiceMock.Setup(service => service.GetAsync(1)).ReturnsAsync(wand);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(wand, okResult.Value);
        }

        [Fact]
        public async Task Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _wandServiceMock.Setup(service => service.GetAsync(1)).ReturnsAsync((GetWandDto)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByFilter_ShouldReturnFilteredWands_ReturnTrue()
        {
            // Arrange
            var filter = new WandFilterDto { WoodType = WoodType.Holly };
            var wands = new List<GetWandDto>
            {
                new GetWandDto(1, WoodType.Holly, 12.5m, WandCore.PhoenixFeather, Flexibility.Supple, 100m, false)
            };
            _wandServiceMock.Setup(service => service.GetWandsByAsync(filter)).ReturnsAsync(wands);

            // Act
            var result = await _controller.GetBy(filter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(wands, okResult.Value);
        }

        [Fact]
        public async Task GetByFilter_ShouldReturnNotFound()
        {
            // Arrange
            var filter = new WandFilterDto { WoodType = WoodType.Holly };
            _wandServiceMock.Setup(service => service.GetWandsByAsync(filter)).ReturnsAsync(new List<GetWandDto>());

            // Act
            var result = await _controller.GetBy(filter);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No wands found matching the provided filters.", notFoundResult.Value);
        }

        [Fact]
        public async Task Add_ShouldReturnOkWithCreatedWand()
        {
            // Arrange
            var createWandDto = new CreateWandDto(WoodType.Holly, 12.5m, WandCore.PhoenixFeather, Flexibility.Supple, 100m);
            var createdWand = new GetWandDto(1, WoodType.Holly, 12.5m, WandCore.PhoenixFeather, Flexibility.Supple, 100m, false);
            _wandServiceMock.Setup(service => service.AddAsync(createWandDto)).ReturnsAsync(createdWand);

            // Act
            var result = await _controller.Post(createWandDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(createdWand, okResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOkWithDeletedWand()
        {
            // Arrange
            var deletedWand = new GetWandDto(1, WoodType.Holly, 12.5m, WandCore.PhoenixFeather, Flexibility.Supple, 100m, true);
            _wandServiceMock.Setup(service => service.DeleteWand(1)).ReturnsAsync(deletedWand);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(deletedWand, okResult.Value);
        }
    }
}
