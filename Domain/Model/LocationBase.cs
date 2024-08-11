namespace Domain.Model
{
    public class LocationBase
    {
        public LocationBase() { }
        public LocationBase(double longitude, double latitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

    }
}
