using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace VacationRental.Api.Tests.Calendar
{
    public class GetCalendarTests : IClassFixture<InMemorySeedDataFixture>
    {
        private readonly InMemorySeedDataFixture _fixture;

        public GetCalendarTests(InMemorySeedDataFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetCalendar_ReturnCalendarModel()
        {
            // Arrange
            var units = 2;
            var preparationTimeInDays = 1;
            var rentalModel = await _fixture.CreateRental(units, preparationTimeInDays);
            var startDate = new DateTime(2000, 01, 02);
            var nights = 2;

            var bookingModel = await _fixture.CreateBooking(rentalModel.Id, startDate, nights);
            var bookingUnitId = (await _fixture._vrContext.Bookings
                .Include(b => b.Unit)
                .FirstOrDefaultAsync(b => b.Id == bookingModel.Id))
                .UnitId;
            var bookingModel2 = await _fixture.CreateBooking(rentalModel.Id, startDate.AddDays(1), nights);
            var bookingUnitId2 = (await _fixture._vrContext.Bookings
                .Include(b => b.Unit)
                .FirstOrDefaultAsync(b => b.Id == bookingModel2.Id))
                .UnitId;

            var calendarNights = 7;

            // Act
            var result = await _fixture.GetCalendar(rentalModel.Id, startDate.AddDays(-1), calendarNights);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rentalModel.Id, result.RentalId);
            Assert.Equal(calendarNights, result.Dates.Count);

            Assert.Equal(new DateTime(2000, 01, 01), result.Dates[0].Date);
            Assert.Empty(result.Dates[0].Bookings);
            Assert.Empty(result.Dates[0].PreparationTimes);

            Assert.Equal(new DateTime(2000, 01, 02), result.Dates[1].Date);
            Assert.Single(result.Dates[1].Bookings);
            Assert.Contains(result.Dates[1].Bookings, x => x.Id == bookingModel.Id);
            Assert.Empty(result.Dates[1].PreparationTimes);

            Assert.Equal(new DateTime(2000, 01, 03), result.Dates[2].Date);
            Assert.Equal(2, result.Dates[2].Bookings.Count);
            Assert.Contains(result.Dates[2].Bookings, x => x.Id == bookingModel.Id);
            Assert.Contains(result.Dates[2].Bookings, x => x.Id == bookingModel2.Id);
            Assert.Empty(result.Dates[1].PreparationTimes);

            Assert.Equal(new DateTime(2000, 01, 04), result.Dates[3].Date);
            Assert.Equal(2, result.Dates[3].Bookings.Count);
            Assert.Contains(result.Dates[3].Bookings, x => x.Id == bookingModel.Id);
            Assert.Contains(result.Dates[3].Bookings, x => x.Id == bookingModel2.Id);
            Assert.Empty(result.Dates[1].PreparationTimes);

            Assert.Equal(new DateTime(2000, 01, 05), result.Dates[4].Date);
            Assert.Single(result.Dates[4].Bookings);
            Assert.Contains(result.Dates[4].Bookings, x => x.Id == bookingModel2.Id);
            Assert.Contains(result.Dates[4].PreparationTimes, x => x.Unit == bookingUnitId);

            Assert.Equal(new DateTime(2000, 01, 06), result.Dates[5].Date);
            Assert.Empty(result.Dates[5].Bookings);
            Assert.Contains(result.Dates[5].PreparationTimes, x => x.Unit == bookingUnitId2);

            Assert.Equal(new DateTime(2000, 01, 07), result.Dates[6].Date);
            Assert.Empty(result.Dates[6].Bookings);
            Assert.Empty(result.Dates[6].PreparationTimes);
        }
    }
}
