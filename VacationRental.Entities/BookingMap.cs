using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VacationRental.Entities
{
    public class BookingMap : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Unit)
                   .WithMany()
                   .HasForeignKey(p => p.UnitId);
        }
    }
}
