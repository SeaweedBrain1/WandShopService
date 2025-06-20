using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WandShopService.Controllers;
using WandShop.Application.Service;
using WandShop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WandShop.Tests.Controllers
{
    public class FlexibilityControllerTests
    {   
        private readonly Mock<IFlexibilityService> _mockService;
        private readonly FlexibilityController _controller;

        public FlexibilityControllerTests()
        {
            _mockService = new Mock<IFlexibilityService>();
            _controller = new FlexibilityController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllFlexibilities_WhenFlexibilitiesExist_ReturnsOkWithList()
        {
            var list = new List<Flexibility> { new() { Id = 1, Name = "Soft" } };
            _mockService.Setup(s => s.GetAllFlexibilitiesAsync()).ReturnsAsync(list);

            var result = await _controller.GetAllFlexibilities();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(list, ok.Value);
        }

        [Fact]
        public async Task GetFlexibility_WhenFlexibilityExists_ReturnsOkWithItem()
        {
            var flex = new Flexibility { Id = 1, Name = "Firm" };
            _mockService.Setup(s => s.GetFlexibilityAsync(1)).ReturnsAsync(flex);

            var result = await _controller.GetFlexibility(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(flex, ok.Value);
        }

        [Fact]
        public async Task GetFlexibility_WhenFlexibilityDoesNotExist_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetFlexibilityAsync(999)).ReturnsAsync((Flexibility)null);

            var result = await _controller.GetFlexibility(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddFlexibility_ValidFlexibility_ReturnsCreatedAt()
        {
            var input = new Flexibility { Name = "Stiff" };
            var created = new Flexibility { Id = 10, Name = "Stiff" };
            _mockService.Setup(s => s.AddFlexibilityAsync(input)).ReturnsAsync(created);

            var result = await _controller.AddFlexibility(input);

            var action = Assert.IsType<ActionResult<Flexibility>>(result);
            var createdAt = Assert.IsType<CreatedAtActionResult>(action.Result);
            Assert.Equal("GetFlexibility", createdAt.ActionName);
            Assert.Equal(created, createdAt.Value);
        }

        [Fact]
        public async Task UpdateFlexibility_IdMismatch_ReturnsBadRequest()
        {
            var flex = new Flexibility { Id = 5, Name = "Rigid" };

            var result = await _controller.UpdateFlexibility(6, flex);

            var action = Assert.IsType<ActionResult<Flexibility>>(result);
            Assert.IsType<BadRequestResult>(action.Result);
        }

        [Fact]
        public async Task UpdateFlexibility_Valid_ReturnsOkWithUpdated()
        {
            var input = new Flexibility { Id = 5, Name = "Springy" };
            _mockService.Setup(s => s.UpdateFlexibilityAsync(input)).ReturnsAsync(input);

            var result = await _controller.UpdateFlexibility(5, input);

            var action = Assert.IsType<ActionResult<Flexibility>>(result);
            var ok = Assert.IsType<OkObjectResult>(action.Result);
            Assert.Equal(input, ok.Value);
        }
    }
}