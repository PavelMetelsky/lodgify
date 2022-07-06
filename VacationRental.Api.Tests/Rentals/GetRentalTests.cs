using System.Threading.Tasks;
using Xunit;

namespace VacationRental.Api.Tests.Rentals
{
    public class GetRentalTests : IClassFixture<InMemorySeedDataFixture>
    {
        private readonly InMemorySeedDataFixture _fixture;

        public GetRentalTests(InMemorySeedDataFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetRentals_ReturnRental()
        {
            // Arrange
            var units = 10;
            var preparationTimeInDays = 3;
            var rentalModel = await _fixture.CreateRental(units, preparationTimeInDays);

            // Act
            var result = await _fixture.GetRental(rentalModel.Id);

            // Assert
            Assert.True(result.Id == rentalModel.Id);
            Assert.True(result.Units == units);
            Assert.True(result.PreparationTimeInDays == preparationTimeInDays);
        }
    }
}