using Microsoft.EntityFrameworkCore;
using VacationRental.Entities;

namespace VacationRental.Database
{
    public class VRContext : DbContext
    {
        public VRContext(DbContextOptions<VRContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Rental> Rentals { get; set; }
    }
}
