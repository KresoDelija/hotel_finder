namespace Domain.Model
{
    public class Hotel : HotelDetails
    {
        public int Id { get; set; }

        public Hotel() { }

        public async Task<Hotel> Create(IHotelService hotelService)
        {
            if (Name.Length < 3) 
            {
                throw new ArgumentException("Name is less then 3 characters");
            }

            if (Price <= 0)
            {
                throw new ArgumentException("Price is less or equal 0");
            }

            var existingHotels = await hotelService.GetHotels();

            if (existingHotels.Any(hotelInDB =>
            (
            hotelInDB.Name == Name
            && hotelInDB.Location.Longitude == Location.Longitude
            && hotelInDB.Location.Latitude == Location.Latitude
            && hotelInDB.Price == Price
            )
            ))
            {
                throw new ArgumentException("Record already exists");
            }

            return await hotelService.CreateHotel(Name, Price, Location.Longitude, Location.Latitude);
        }

        public async Task<Hotel> Update(IHotelService hotelService)
        {
            if (Name.Length < 3)
            {
                throw new ArgumentException("Name is less then 3 characters");
            }

            if (Price <= 0)
            {
                throw new ArgumentException("Price is less or equal 0");
            }

            var existingHotel = await hotelService.GetHotel(Id);

            if (existingHotel == null) 
            {
                throw new ArgumentException("Hotel doesnt exist");
            }

            var existingHotels = await hotelService.GetHotels();

            if (existingHotels.Any(hotelInDB => hotelInDB.Id != existingHotel.Id &&
            (
            hotelInDB.Name == existingHotel.Name 
            && hotelInDB.Location.Longitude == existingHotel.Location.Longitude 
            && hotelInDB.Location.Latitude == existingHotel.Location.Latitude
            && hotelInDB.Price == existingHotel.Price
            )
            ))
            {
                throw new ArgumentException("Record already exists");
            }

            return await hotelService.UpdateHotel(this);
        }

        public async Task<bool> Delete(IHotelService hotelService)
        {
            var existingHotel = await hotelService.GetHotel(Id);

            if (existingHotel == null)
            {
                throw new ArgumentException("Hotel doesnt exist");
            }

            return await hotelService.DeleteHotel(Id);
        }


        public async Task<Hotel> LoadData(IHotelService hotelService)
        {
            var existingHotel = await hotelService.GetHotel(Id);

            if (existingHotel == null)
            {
                throw new ArgumentException("Hotel doesnt exist");
            }
            return existingHotel;
        }

        public async Task<List<Hotel>> GetAll(IHotelService hotelService)
        {
            return await hotelService.GetHotels();
        }

    }
}
