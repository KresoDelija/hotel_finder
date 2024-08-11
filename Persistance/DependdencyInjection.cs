using Domain;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistance
{
    public static class DependdencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<LocationConfiguration, LocationConfiguration >();
            string connectionString = configuration.GetConnectionString("HotelDB");
            services.AddDbContext<HotelContext>(options => 
            options.UseSqlServer(connectionString, opt=>opt.UseNetTopologySuite())

            );
            services.AddScoped<IHotelService, HotelService>();

                        
            return services;
        }
    }
}
