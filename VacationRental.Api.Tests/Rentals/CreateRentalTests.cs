using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace VacationRental.Api.Tests.Rentals
{
    public class CreateRentalTests : IClassFixture<InMemorySeedDataFixture>
    {
        private readonly InMemorySeedDataFixture _fixture;

        public CreateRentalTests(InMemorySeedDataFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(1, 25)]
        [InlineData(2, 10)]
        [InlineData(3, 5)]
        public async Task CreateRental_ReturnRental(int units, int preparationTimeInDays)
        {
            // Act
            var rentalModel = await _fixture.CreateRental(units, preparationTimeInDays);

            // Assert
            var rentalEntity = await _fixture._vrContext.Rentals
                .Include(r => r.Units)
                .FirstOrDefaultAsync(b => b.Id == rentalModel.Id);

            Assert.NotNull(rentalEntity);
            Assert.True(rentalEntity.Id == rentalModel.Id);
            Assert.True(rentalEntity.Units.Count == units);
            Assert.True(rentalEntity.PreparationTimeInDays == preparationTimeInDays);
        }

    }
}
