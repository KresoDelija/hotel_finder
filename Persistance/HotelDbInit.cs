using NetTopologySuite;
using NetTopologySuite.Geometries;
using Domain.Model;

namespace Persistance
{
    public static class HotelDbInit
    {
        public static void Initialize(HotelContext hotelContext, LocationConfiguration configuration) 
        {
            hotelContext.Database.EnsureCreated();
            
            if (!hotelContext.Hotels.Any())
            {                
                var locationFactory = NtsGeometryServices.Instance.CreateGeometryFactory(configuration.SpatialReferenceSystem);

                var hotels = new DataModels.Hotel[]
                {
                    new DataModels.Hotel(){ Name = "Hotel Jarun", Price = 100, Location = locationFactory.CreatePoint(new Coordinate(45.79117, 15.92954)) },
                    new DataModels.Hotel(){ Name = "Hotel Antunović", Price = 150, Location = locationFactory.CreatePoint(new Coordinate(45.799308, 15.9143077)) },
                    new DataModels.Hotel(){ Name = "Hotel Aristos", Price = 250, Location = locationFactory.CreatePoint(new Coordinate(45.7993754, 15.8837098)) }
                };

                hotelContext.Hotels.AddRange(hotels);
                hotelContext.SaveChanges();

            }
        }


    }
}
