using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using VacationRental.Database;
using Xunit;

namespace VacationRental.Api.Tests.Rentals
{
    public class UpdateRentalTests : IClassFixture<InMemorySeedDataFixture>
    {
        private readonly InMemorySeedDataFixture _fixture;

        public UpdateRentalTests(InMemorySeedDataFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(1, 25)]
        [InlineData(2, 10)]
        [InlineData(3, 5)]
        public async Task UpdateRental_ReturnRental(int units, int preparationTimeInDays)
        {
            // Arrange
            var unitsInit = 10;
            var preparationTimeInDaysInit = 3;
            var rentalModel = await _fixture.CreateRental(unitsInit, preparationTimeInDaysInit);

            // Act
            var updatedRentalModel = await _fixture.UpdateRental(rentalModel.Id, units, preparationTimeInDays);

            // Assert
            var result = await _fixture.GetRental(updatedRentalModel.Id);

            Assert.NotNull(result);
            Assert.True(result.Id == rentalModel.Id);
            Assert.True(updatedRentalModel.Id == rentalModel.Id);
            Assert.True(result.Units == units);
            Assert.True(result.PreparationTimeInDays == preparationTimeInDays);
        }

        [Theory]
        [InlineData(1, 3)]
        public async Task UpdateRental_UpdateUnitsFailed(int units, int preparationTimeInDays)
        {
            // Arrange
            var unitsInit = 2;
            var preparationTimeInDaysInit = 3;
            var rentalModel = await _fixture.CreateRental(unitsInit, preparationTimeInDaysInit);

            var startDate = new DateTime(2001, 01, 01);
            var nights = 3;
            var bookingModel = await _fixture.CreateBooking(rentalModel.Id, startDate, nights);
            var bookingModel2 = await _fixture.CreateBooking(rentalModel.Id, startDate, nights);

            // Act
            await Assert.ThrowsAsync<ApplicationException>(() =>
                  _fixture.UpdateRental(rentalModel.Id, units, preparationTimeInDays));
        }

        [Theory]
        [InlineData(3, 3)]
        public async Task UpdateRental_UpdateUnitsSuccessfully(int units, int preparationTimeInDays)
        {
            // Arrange
            var unitsInit = 2;
            var preparationTimeInDaysInit = 3;
            var rentalModel = await _fixture.CreateRental(unitsInit, preparationTimeInDaysInit);

            var startDate = new DateTime(2001, 01, 01);
            var nights = 3;
            var bookingModel = await _fixture.CreateBooking(rentalModel.Id, startDate, nights);
            var bookingModel2 = await _fixture.CreateBooking(rentalModel.Id, startDate, nights);

            // Act
            var updatedRentalModel = await _fixture.UpdateRental(rentalModel.Id, units, preparationTimeInDays);

            // Assert
            Assert.NotNull(updatedRentalModel);
        }
    }
}
