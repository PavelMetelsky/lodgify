using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Commands.Bookings.CreateBooking;
using VacationRental.BusinessLogic.Models;
using Xunit;

namespace VacationRental.Api.Tests.Bookings
{
    public class CreateBookingTests : IClassFixture<InMemorySeedDataFixture>
    {
        private readonly InMemorySeedDataFixture _fixture;

        public CreateBookingTests(InMemorySeedDataFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CreateBooking_NightsMustBePositive()
        {
            // Arrange
            var units = 1;
            var preparationTimeInDays = 3;
            var rentalModel = await _fixture.CreateRental(units, preparationTimeInDays);
            var startDate = new DateTime(2001, 01, 01);
            var nights = 0;

            // Act
            await Assert.ThrowsAsync<ApplicationException>(() =>
                 _fixture.CreateBooking(rentalModel.Id, startDate, nights));
        }

        [Fact]
        public async Task CreateBooking_RentalNotFound()
        {
            // Arrange
            var startDate = new DateTime(2001, 01, 01);
            var nights = 0;

            // Act
            await Assert.ThrowsAsync<ApplicationException>(() =>
                 _fixture.CreateBooking(5, startDate, nights));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(35)]
        public async Task CreateBooking_ReturnBooking(int nights)
        {
            // Arrange
            var units = 10;
            var preparationTimeInDays = 3;
            var rentalModel = await _fixture.CreateRental(units, preparationTimeInDays);
            var startDate = new DateTime(2001, 01, 01);

            // Act
            var bookingModel = await _fixture.CreateBooking(rentalModel.Id, startDate, nights);

            // Assert
            var bookingEntity = await _fixture._vrContext.Bookings
                            .Include(b => b.Unit)
                            .FirstOrDefaultAsync(b => b.Id == bookingModel.Id);

            Assert.NotNull(bookingEntity);
            Assert.True(bookingEntity.Id == bookingModel.Id);
            Assert.True(bookingEntity.Unit.RentalId == rentalModel.Id);
            Assert.True(bookingEntity.Start == startDate);
            Assert.True(bookingEntity.End == startDate.AddDays(nights));
        }

        [Fact]
        public async Task CreateBooking_TheSameTimeWithOneUnit_Failed()
        {
            // Arrange
            var units = 1;
            var preparationTimeInDays = 3;
            var rentalModel = await _fixture.CreateRental(units, preparationTimeInDays);
            var startDate = new DateTime(2001, 01, 01);
            var nights = 3;

            // Act
            var bookingModel = await _fixture.CreateBooking(rentalModel.Id, startDate, nights);

            // Assert
            await Assert.ThrowsAsync<ApplicationException>(() =>
                 _fixture.CreateBooking(rentalModel.Id, startDate, nights));
        }

        [Fact]
        public async Task CreateBooking_TheSameTimeWithTwoUnit_Successfully()
        {
            // Arrange
            var units = 2;
            var preparationTimeInDays = 3;
            var rentalModel = await _fixture.CreateRental(units, preparationTimeInDays);
            var startDate = new DateTime(2001, 01, 01);
            var nights = 3;

            // Act
            var bookingModel = await _fixture.CreateBooking(rentalModel.Id, startDate, nights);

            // Assert
            var bookingModel2 = await _fixture.CreateBooking(rentalModel.Id, startDate, nights);

            Assert.NotNull(bookingModel);
            Assert.NotNull(bookingModel2);
            Assert.True(bookingModel.Id != bookingModel2.Id);
        }

        [Fact]
        public async Task CreateBooking_OverBookingFailure()
        {
            // Arrange
            var units = 1;
            var preparationTimeInDays = 3;
            var rentalModel = await _fixture.CreateRental(units, preparationTimeInDays);
            var startDate = new DateTime(2002, 01, 01);
            var nights = 3;

            // Act
            var bookingModel = await _fixture.CreateBooking(rentalModel.Id, startDate, nights);

            // Assert
            //We need disable eager loading because we use Filtered includes
            (_fixture._vrContext as FakeVRContext).UnloadDataFromMemory();
            await Assert.ThrowsAsync<ApplicationException>(() =>
                 _fixture.CreateBooking(rentalModel.Id, startDate.AddDays(1), nights));
        }

        [Fact]
        public async Task CreateBooking_Successfully()
        {
            // Arrange
            var units = 1;
            var preparationTimeInDays = 3;
            var rentalModel = await _fixture.CreateRental(units, preparationTimeInDays);
            var startDate = new DateTime(2002, 01, 01);
            var nights = 3;

            // Act
            var bookingModel = await _fixture.CreateBooking(rentalModel.Id, startDate, nights);
            //We need disable eager loading because we use Filtered includes
            (_fixture._vrContext as FakeVRContext).UnloadDataFromMemory();
            var bookingModel2 = await _fixture.CreateBooking(rentalModel.Id, startDate.AddDays(100), nights);

            // Assert
            var bookingEntity = await _fixture._vrContext.Bookings
                            .Include(b => b.Unit)
                            .FirstOrDefaultAsync(b => b.Id == bookingModel2.Id);

            Assert.NotNull(bookingEntity);
            Assert.True(bookingEntity.Id == bookingModel2.Id);
            Assert.True(bookingEntity.Unit.RentalId == rentalModel.Id);
            Assert.True(bookingEntity.Start == startDate.AddDays(100));
            Assert.True(bookingEntity.End == startDate.AddDays(100 + nights));
        }
    }
}
