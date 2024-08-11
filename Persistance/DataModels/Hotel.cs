using System.ComponentModel.DataAnnotations;
using Domain.Model;
using NetTopologySuite.Geometries;

namespace Persistance.DataModels
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }

        [Required]
        public Point Location { get; set; }

        public Domain.Model.Hotel ToDomainModel()
        {
            return new Domain.Model.Hotel() { Id = Id, Name = Name, Price = Price, Location = new LocationBase(Location.X, Location.Y) };
        }        
    }
}
