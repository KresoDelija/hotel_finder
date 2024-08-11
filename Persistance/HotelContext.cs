using Microsoft.EntityFrameworkCore;
using Persistance.DataModels;

namespace Persistance
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> contextOptions) : base(contextOptions) { }

        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>().ToTable(nameof(Hotel));
        }
    }
}
