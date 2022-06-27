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
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=.;Database=VacationRentalDb;Trusted_Connection=True;");
        //}

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Rental> Rentals { get; set; }
    }
}
