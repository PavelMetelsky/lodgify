using Microsoft.EntityFrameworkCore;
using System;
using VacationRental.Database;

namespace VacationRental.Api.Tests
{
    public class InMemorySeedDataFixture : IDisposable
    {
        public VRContext _vrContext { get; private set; }

        public InMemorySeedDataFixture()
        {
            var options = new DbContextOptionsBuilder<VRContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            _vrContext = new VRContext(options);
        }

        public void AddRental(int units, int preparationTimeInDays)
        {
            _vrContext.Rentals.Add(new Entities.Rental {Units = units, PreparationTimeInDays = preparationTimeInDays });
            _vrContext.SaveChanges();
        }

        public void Dispose()
        {
            _vrContext.Dispose();
        }
    }
}
