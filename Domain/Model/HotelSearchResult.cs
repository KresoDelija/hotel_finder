namespace Domain.Model
{
    public class HotelSearchResult
    {
        public int? PageNumber { get; init; }
        public int? PageSize { get; init; }
        public int? TotalPages { get; init; }
        public int TotalRecords { get; init; }
        public List<HotelSearchResultItem> Hotels { get; init; }
        public HotelSearchResult() { }

       
        public async Task<HotelSearchResult> Search(IHotelService hotelService, LocationBase location, int? pageSize, int? pageNumber)
        {

            return await hotelService.GetHotels(location.Longitude, location.Latitude, pageSize, pageNumber);
        }
    }
}
