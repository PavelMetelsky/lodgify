using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VacationRental.Entities
{
    public class UnitMap : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Rental)
                   .WithMany()
                   .HasForeignKey(p => p.RentalId);
        }
    }
}
