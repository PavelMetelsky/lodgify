using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models.Rentals;
using VacationRental.BusinessLogic.Queries.Rentals;
using Xunit;

namespace VacationRental.Api.Tests.Rentals
{
    // In the same way, you can test each API request(Query or Command) separately in a separate file
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
            _fixture.AddRental(2, 10);

            // Act
            var result = await GetRental();

            // Assert
            Assert.True(result.Units == 2);
            Assert.True(result.PreparationTimeInDays == 10);
        }

        private async Task<RentalViewModel> GetRental()
        {
            var request = new GetRentalsQuery { RentalId = 1};
            var handler = new GetRentalsHandler(_fixture._vrContext);

            return await handler.Handle(request, CancellationToken.None);
        }
    }
}