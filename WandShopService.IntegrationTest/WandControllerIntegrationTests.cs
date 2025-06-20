using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using WandShop.Domain.Enums;
using WandShop.Domain.Models;
using WandShop.Domain.Repositories;
using Xunit;

public class WandControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    public WandControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // pobranie dotychczasowej konfiguracji bazy danych
                    var dbContextOptions = services
                        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DbContext>));

                    // usunięcie dotychczasowej konfiguracji bazy danych
                    services.Remove(dbContextOptions);

                    // Stworzenie nowej bazy danych
                    services
                        .AddDbContext<DbContext>(options => options.UseInMemoryDatabase("MyDBForTest"));

                });
            });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsListOfProducts()
    {


        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            dbContext.Wands.RemoveRange(dbContext.Wands);
            dbContext.Flexibilities.RemoveRange(dbContext.Flexibilities);
            await dbContext.SaveChangesAsync();

            var electronics = new Flexibility { Name = "test" };
            dbContext.Flexibilities.Add(electronics);
            await dbContext.SaveChangesAsync();

            dbContext.Wands.Add(new Wand
            {
                Id = 1,
                Price = 100.0m,
                Flexibility = electronics,
                Core = WandCore.PhoenixFeather,
                WoodType = WoodType.Holly,
                Length = 11.0m,
                Deleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = Guid.NewGuid(),
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = Guid.NewGuid()
            }
            );
            await dbContext.SaveChangesAsync();

        }

        var response = await _client.GetAsync("/api/Wand");
        response.EnsureSuccessStatusCode();

        var responseData = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer.Deserialize<List<Wand>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(products);
        Assert.Single(products);
        Assert.Equal(1, products[0].Id);
    }
}