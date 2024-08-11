using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Persistance
{
    public class HotelService : Domain.IHotelService
    {
        protected readonly HotelContext _context;
        protected readonly LocationConfiguration _locationConfiguration;
        public HotelService(HotelContext context, IOptions<LocationConfiguration> locationConfiguration)
        {
            _context = context;
            _locationConfiguration = locationConfiguration.Value;
        }
        public async Task<Hotel> CreateHotel(string name, double price, double longitude, double latitude)
        {
            var newHotel = new DataModels.Hotel();
            var locationFactory = NtsGeometryServices.Instance.CreateGeometryFactory(_locationConfiguration.SpatialReferenceSystem);            
            var hotelLocation = locationFactory.CreatePoint(new Coordinate(longitude, latitude));
            if (!hotelLocation.IsValid)
            {
                throw new ArgumentOutOfRangeException("Hotel location is not valid");
            }
            newHotel.Name = name;
            newHotel.Price = price;
            newHotel.Location = hotelLocation;
            _context.Hotels.Add(newHotel);
            var insert = await _context.SaveChangesAsync();

            return newHotel.ToDomainModel();
        }

        public async Task<bool> DeleteHotel(int id)
        {
            var hotelForDelete = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            if(hotelForDelete!=null)
            {
                _context.Hotels.Remove(hotelForDelete);
                _context.SaveChanges();
            }
            return true;
        }

        public async Task<Hotel> GetHotel(int id)
        {
            var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            if (hotel != null)
            {
                return hotel.ToDomainModel();
            }
            return null;
        }

        public async Task<List<Hotel>> GetHotels()
        {
            var allHotels = await _context.Hotels
                .Select(hotel => hotel.ToDomainModel())
                .ToListAsync();

            return allHotels;
        }

        public async Task<HotelSearchResult> GetHotels(double longitude, double latitude, int? pageSize, int? currentPage)
        {            
            var locationFactory = NtsGeometryServices.Instance.CreateGeometryFactory(_locationConfiguration.SpatialReferenceSystem);
            var currentLocation = locationFactory.CreatePoint( new Coordinate(longitude, latitude));
            if (!currentLocation.IsValid)
            {
                throw new ArgumentOutOfRangeException("Search location is not valid");
            }
            var totalRecords = await _context.Hotels.CountAsync();
            if (pageSize == null || currentPage == null)
            {
                pageSize = _locationConfiguration.MaxPageSize;
                currentPage = 0;
            }
            var offset = pageSize.Value * currentPage.Value;

            var projectedCurrentLocation = currentLocation.ProjectTo(2855);
            var orderedHotels = await _context.Hotels
                .OrderBy(hotel => hotel.Location.Distance(currentLocation))
                .ThenBy(hotel => hotel.Price)
                .Skip(offset)
                .Take(pageSize.Value)
                .Select(hotel => new HotelSearchResultItem()
                {
                    Name = hotel.Name,
                    Price = hotel.Price,
                    Distance = hotel.Location.ProjectTo(2855).Distance(projectedCurrentLocation)
                })
                .ToListAsync();

            var totalPages = pageSize > 0 ? (totalRecords / pageSize) + 1 : 0;

            return new HotelSearchResult() 
            { 
                Hotels = orderedHotels, 
                TotalPages = totalPages, 
                PageNumber= currentPage, 
                PageSize = pageSize, 
                TotalRecords = totalRecords 
            };  
        }

        public async Task<Hotel> UpdateHotel(Hotel hotel)
        {
            var existinfgHotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == hotel.Id);
            if (existinfgHotel != null)
            {
                existinfgHotel.Name = hotel.Name;
                existinfgHotel.Price = hotel.Price;
                existinfgHotel.Location.X = hotel.Location.Longitude;
                existinfgHotel.Location.Y = hotel.Location.Latitude;
                _context.SaveChanges();
            }
            return null;
        }
    }

    
}
