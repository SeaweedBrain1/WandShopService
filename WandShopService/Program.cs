
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using System.Text;
using WandShop.Application.Service;
using WandShop.Domain.Enums;
using WandShop.Domain.Models;
using WandShop.Domain.Models.Dto;
using WandShop.Domain.Repositories;
using WandShop.Domain.Repository;
using WandShop.Domain.Seeders;
using WandShop.Infrastructure.Converters;



namespace WandShopService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        //builder.Services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TestDb"), ServiceLifetime.Transient);

        if (builder.Environment.IsEnvironment("IntegrationTest"))
        {
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        }
        else
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connectionString), ServiceLifetime.Transient);
        }


        //var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        //builder.Services.AddDbContext<DataContext>(options =>
        //    options.UseSqlServer(connectionString), ServiceLifetime.Transient);

        builder.Services.AddAutoMapper(typeof(WandProfile).Assembly);


        var rsa = RSA.Create();
        rsa.ImportFromPem(File.ReadAllText("./data/public.key"));

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(File.ReadAllText("./data/public.key"));
                var publicKey = new RsaSecurityKey(rsa);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "WandShopNetProject",
                    ValidAudience = "WandShop",
                    IssuerSigningKey = publicKey
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));
            options.AddPolicy("EmployeeOnly", policy =>
                policy.RequireRole("Employee"));
            options.AddPolicy("ClientOnly", policy =>
                policy.RequireRole("Client"));
            options.AddPolicy("ClientEmployeeOrAdmin", policy =>
                policy.RequireRole("Client", "Employee", "Admin"));
        });

        builder.Services.AddScoped<IWandRepository, WandRepository>();
        builder.Services.AddScoped<IFlexibilityRepository, FlexibilityRepository>();
        builder.Services.AddScoped<IWandService, WandService>();
        builder.Services.AddScoped<IFlexibilityService, FlexibilityService>();

        //builder.Services.AddControllers();
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonEnumDisplayConverter<WoodType>());
                options.JsonSerializerOptions.Converters.Add(new JsonEnumDisplayConverter<WandCore>());
            });


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Wpisz token w formacie: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
              {
                {
                  new OpenApiSecurityScheme
                  {
                    Reference = new OpenApiReference
                      {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                      },
                      Scheme = "oauth2",
                      Name = "Bearer",
                      In = ParameterLocation.Header,

                    },
                    new List<string>()
                  }
                });
         });

        builder.Services.AddScoped<IWandSeeder, WandSeeder>();

        //builder.WebHost.UseUrls("http://*:8080");


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        //using (var scope = app.Services.CreateScope())
        //{
        //    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
        //    await db.Database.MigrateAsync();
        //    var seeder = scope.ServiceProvider.GetRequiredService<IWandSeeder>();
        //    await seeder.Seed();
        //}

        if (!builder.Environment.IsEnvironment("IntegrationTest"))
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                await db.Database.MigrateAsync();
                var seeder = scope.ServiceProvider.GetRequiredService<IWandSeeder>();
                await seeder.Seed();
            }
        }

        //var scope = app.Services.CreateScope();
        //var seeder = scope.ServiceProvider.GetRequiredService<IWandSeeder>();
        //await seeder.Seed();

        app.Run();
    }
}

