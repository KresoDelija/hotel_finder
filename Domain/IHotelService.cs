using Domain.Model;

namespace Domain
{
    public interface IHotelService
    {
        Task<Hotel> GetHotel(int id);
        Task<Hotel> CreateHotel(string name, double price, double longitude, double latitude);
        Task<Hotel> UpdateHotel(Hotel hotel);
        Task<bool> DeleteHotel(int id);

        Task<List<Hotel>> GetHotels();
        Task<HotelSearchResult> GetHotels(double longitude, double latitude, int? pagesize, int? currentPage);
    }
}
