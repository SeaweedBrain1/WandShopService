using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using StackExchange.Redis;
using WandShop.Domain.Enums;
using WandShop.Domain.Models;
using WandShop.Domain.Repositories;
using Xunit;

namespace WandShopService.IntegrationTests;

public class FlexibilityControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public FlexibilityControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("IntegrationTest");
            builder.ConfigureServices(services =>
            {
                //var dbContextDescriptor = services.SingleOrDefault(
                //    d => d.ServiceType == typeof(DbContextOptions<DataContext>));
                //if (dbContextDescriptor != null)
                //    services.Remove(dbContextDescriptor);

                //services.AddDbContext<DataContext>(options =>
                //    options.UseInMemoryDatabase("TestDb_Flexibility"));

                var dbContextOptions = services
                    .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataContext>));

                services.Remove(dbContextOptions);

                services
                    .AddDbContext<DataContext>(options => options.UseInMemoryDatabase("MyDBForTest"));
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllFlexibilities_ReturnsFlexibilityList()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            dbContext.Flexibilities.AddRange(
                new Flexibility { Name = "Soft" },
                new Flexibility { Name = "Rigid" }
            );
            await dbContext.SaveChangesAsync();
        }

        var response = await _client.GetAsync("/api/Flexibility");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var flexibilities = JsonSerializer.Deserialize<List<Flexibility>>(responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(flexibilities);
        Assert.Equal(2, flexibilities.Count);
        Assert.Contains(flexibilities, f => f.Name == "Soft");
        Assert.Contains(flexibilities, f => f.Name == "Rigid");
    }
}
