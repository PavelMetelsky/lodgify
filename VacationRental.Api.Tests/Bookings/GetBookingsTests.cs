using System;
using System.Threading.Tasks;
using Xunit;

namespace VacationRental.Api.Tests.Bookings
{
    public class GetBookingsTests : IClassFixture<InMemorySeedDataFixture>
    {
        private readonly InMemorySeedDataFixture _fixture;

        public GetBookingsTests(InMemorySeedDataFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetBooking_ReturnBookingModel()
        {
            // Arrange
            var units = 10;
            var preparationTimeInDays = 3;
            var rentalModel = await _fixture.CreateRental(units, preparationTimeInDays);
            var startDate = new DateTime(2001, 01, 01);
            var nights = 2;
            var bookingModel = await _fixture.CreateBooking(rentalModel.Id, startDate, nights);

            // Act
            var result = await _fixture.GetBookings(bookingModel.Id);

            // Assert
            Assert.True(result.Id == bookingModel.Id);
            Assert.True(result.RentalId == rentalModel.Id);
            Assert.True(result.Nights == nights);
            Assert.True(result.Start == startDate);
        }
    }
}
