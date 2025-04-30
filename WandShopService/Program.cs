
using Microsoft.EntityFrameworkCore;
using WandShop.Application.Service;
using WandShop.Domain.Enums;
using WandShop.Domain.Repositories;
using WandShop.Domain.Repository;
using WandShop.Domain.Seeders;
using WandShop.Infrastructure.Converters;

namespace WandShopService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TestDb"), ServiceLifetime.Transient);

            builder.Services.AddScoped<IWandRepository, WandRepository>();
            builder.Services.AddScoped<IWandService, WandService>();

            //builder.Services.AddControllers();
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonEnumDisplayConverter<WoodType>());
                    options.JsonSerializerOptions.Converters.Add(new JsonEnumDisplayConverter<Flexibility>());
                    options.JsonSerializerOptions.Converters.Add(new JsonEnumDisplayConverter<WandCore>());
                });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IWandSeeder, WandSeeder>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            //using (var scope = app.Services.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            //    await db.Database.MigrateAsync();
            //    var seeder = scope.ServiceProvider.GetRequiredService<IWandSeeder>();
            //    await seeder.Seed();
            //}

            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IWandSeeder>();
            await seeder.Seed();

            app.Run();
        }
    }
}
