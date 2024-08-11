namespace Domain.Model
{
    public class HotelDetails
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public LocationBase Location { get; set; }

        public HotelDetails() { }
       

    }
}
