using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Commands.Bookings.CreateBooking;
using VacationRental.BusinessLogic.Commands.Rentals.CreateRental;
using VacationRental.BusinessLogic.Models;
using VacationRental.BusinessLogic.Models.Bookings;
using VacationRental.BusinessLogic.Models.Rentals;
using VacationRental.BusinessLogic.Queries.Bookings.GetBooking;
using VacationRental.BusinessLogic.Queries.Calendars.GetCalendars;
using VacationRental.BusinessLogic.Queries.Rentals.GetRental;
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

        public async Task<RentalViewModel> GetRental(int rentalId)
        {
            var request = new GetRentalsQuery { RentalId = rentalId };
            var handler = new GetRentalsHandler(_vrContext);

            return await handler.Handle(request, CancellationToken.None);
        }

        public async Task<ResourceIdViewModel> CreateRental(int units, int preparationTimeInDays)
        {
            var request = new CreateRentalCommand
            {
                Units = units,
                PreparationTimeInDays = preparationTimeInDays
            };
            var handler = new CreateRentalHandler(_vrContext);

            return await handler.Handle(request, CancellationToken.None);
        }

        public async Task<ResourceIdViewModel> UpdateRental(int rentalId, int units, int preparationTimeInDays)
        {
            var request = new UpdateRentalCommand
            {
                RentalId = rentalId,
                Units = units,
                PreparationTimeInDays = preparationTimeInDays
            };
            var handler = new UpdateRentalHandler(_vrContext);

            return await handler.Handle(request, CancellationToken.None);
        }

        public async Task<BookingViewModel> GetBookings(int bookingId)
        {
            var request = new GetBookingsQuery { BookingId = bookingId };
            var handler = new GetBookingsHandler(_vrContext);

            return await handler.Handle(request, CancellationToken.None);
        }

        public async Task<ResourceIdViewModel> CreateBooking(int rentalId, DateTime start, int nights)
        {
            var request = new CreateBookingCommand
            {
                RentalId = rentalId,
                Start = start,
                Nights = nights
            };
            var handler = new CreateBookingHandler(_vrContext);

            return await handler.Handle(request, CancellationToken.None);
        }

        public async Task<CalendarViewModel> GetCalendar(int rentalId, DateTime start, int nights)
        {
            var request = new GetCalendarsQuery
            {
                RentalId = rentalId,
                Start = start,
                Nights = nights
            };
            var handler = new GetCalendarsHandler(_vrContext);

            return await handler.Handle(request, CancellationToken.None);
        }

        public void Dispose()
        {
            _vrContext.Dispose();
        }
    }
}
