
using Domain;
using Domain.Model;
using HotelFinderAPI.Model;
using Microsoft.Extensions.Configuration;
using Persistance;

namespace HotelFinder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true);

            var configuration = configurationBuilder.Build();
            var builder = WebApplication.CreateBuilder(args);

            AddServices(builder, configuration);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<HotelContext>();
                var locationConfiguration = new LocationConfiguration();
                configuration.GetSection(nameof(LocationConfiguration)).Bind(locationConfiguration);
                HotelDbInit.Initialize(context, locationConfiguration);
            }

            ConfigurePipeline(app, configuration);

            app.Run();
        }

        private static void ConfigurePipeline(WebApplication app, IConfiguration configuration)
        {
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseCors();
            app.MapControllers();           
        }

        private static void AddServices(WebApplicationBuilder builder, IConfiguration configuration)
        {
            var appSetings = new AppSetings();
            configuration.GetSection(nameof(AppSetings)).Bind(appSetings);
            builder.Services.Configure<LocationConfiguration>(configuration.GetSection(nameof(LocationConfiguration)));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.DescribeAllParametersInCamelCase();
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                options.CustomOperationIds(e => e.ActionDescriptor.DisplayName);
                options.EnableAnnotations();
                options.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath}");
                options.CustomSchemaIds(s => s.FullName.Replace("+", "."));
            });
            builder.Services.AddCors(options=>
            {
                options.AddDefaultPolicy(builder => builder
                //.WithOrigins(appSettings.AllowedOrigins));
                 .WithOrigins(appSetings.AllowedOrigins.ToArray())
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("Content-Disposition"));

            });

            builder.Services.AddPersistance(configuration);
        }
    }
}
