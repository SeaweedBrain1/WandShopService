using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WandShop.Application.Service;
using WandShop.Domain.Enums;
using WandShop.Domain.Models.Dto;
using WandShopService.Controllers;
using Xunit;

namespace WandShopService.UnitTests.Controllers;

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
    public async Task GetAll_WhenWandsExist_ReturnsOkWithList()
    {
        var wands = new List<GetWandDto>
        {
            new(1, WoodType.Oak, 14.5m, WandCore.PhoenixFeather, "Supple", 199.99m),
            new(2, WoodType.Yew, 13.0m, WandCore.DragonHeartstring, "Rigid", 249.50m)
        };

        _wandServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(wands);

        var result = await _controller.Get();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(wands, ok.Value);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsOk()
    {
        var wand = new GetWandDto(1, WoodType.Holly, 11.0m, WandCore.UnicornHair, "Flexible", 150.00m);
        _wandServiceMock.Setup(s => s.GetAsync(1)).ReturnsAsync(wand);

        var result = await _controller.Get(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(wand, ok.Value);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        _wandServiceMock.Setup(s => s.GetAsync(99)).ReturnsAsync((GetWandDto)null);

        var result = await _controller.Get(99);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetValid_ReturnsValidWands()
    {
        var wands = new List<GetWandDto>
        {
            new(3, WoodType.Willow, 12.5m, WandCore.PhoenixFeather, "Whippy", 199.99m)
        };

        _wandServiceMock.Setup(s => s.GetAllValidAsync()).ReturnsAsync(wands);

        var result = await _controller.GetValid();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(wands, ok.Value);
    }

    [Fact]
    public async Task GetByFilter_WithNoResults_ReturnsNotFound()
    {
        _wandServiceMock.Setup(s => s.GetWandsByAsync(It.IsAny<WandFilterDto>()))
                        .ReturnsAsync(new List<GetWandDto>());

        var result = await _controller.GetBy(new WandFilterDto());

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No wands found matching the provided filters.", notFound.Value);
    }

    [Fact]
    public async Task Post_ValidWand_ReturnsOk()
    {
        var createDto = new CreateWandDto
        {
            WoodType = WoodType.Maple,
            Length = 10.5m,
            Core = WandCore.DragonHeartstring,
            FlexibilityName = "Stiff",
            Price = 175.0m
        };

        var expectedDto = new GetWandDto(10, createDto.WoodType, createDto.Length, createDto.Core, createDto.FlexibilityName, createDto.Price);

        _wandServiceMock.Setup(s => s.AddAsync(createDto)).ReturnsAsync(expectedDto);

        var result = await _controller.Post(createDto);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedDto, ok.Value);
    }

    [Fact]
    public async Task Put_ExistingWand_UpdatesAndReturnsOk()
    {
        var updateDto = new UpdateWandDto
        {
            WoodType = WoodType.Walnut,
            Length = 12.0m,
            Core = WandCore.UnicornHair,
            FlexibilityName = "Flexible",
            Price = 180.0m
        };

        var updated = new GetWandDto(1, updateDto.WoodType!.Value, updateDto.Length!.Value, updateDto.Core!.Value, updateDto.FlexibilityName, updateDto.Price!.Value);

        _wandServiceMock.Setup(s => s.UpdateAsync(1, updateDto)).ReturnsAsync(updated);

        var result = await _controller.Put(1, updateDto);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(updated, ok.Value);
    }

    [Fact]
    public async Task Delete_ExistingWand_ReturnsOk()
    {
        var deleted = new GetWandDto(5, WoodType.Ebony, 13.2m, WandCore.PhoenixFeather, "Semi-Flexible", 210.00m);

        _wandServiceMock.Setup(s => s.DeleteWand(5)).ReturnsAsync(deleted);

        var result = await _controller.Delete(5);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(deleted, ok.Value);
    }

    [Fact]
    public async Task Delete_NonExistingWand_ReturnsNullInOk()
    {
        _wandServiceMock.Setup(s => s.DeleteWand(999)).ReturnsAsync((GetWandDto)null);

        var result = await _controller.Delete(999);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Null(ok.Value);
    }

    [Fact]
    public async Task Post_WithZeroLength_ReturnsOkButEdgeCase()
    {
        var createDto = new CreateWandDto
        {
            WoodType = WoodType.Oak,
            Length = 0.0m,
            Core = WandCore.DragonHeartstring,
            FlexibilityName = "Brittle",
            Price = 120.00m
        };

        var resultDto = new GetWandDto(12, createDto.WoodType, createDto.Length, createDto.Core, createDto.FlexibilityName, createDto.Price);

        _wandServiceMock.Setup(s => s.AddAsync(createDto)).ReturnsAsync(resultDto);

        var result = await _controller.Post(createDto);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(resultDto, ok.Value);
    }
}
