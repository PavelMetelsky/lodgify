using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models.Bookings;
using VacationRental.Database;

namespace VacationRental.BusinessLogic.Queries.Bookings.GetBooking
{
    public class GetBookingsHandler : IRequestHandler<GetBookingsQuery, BookingViewModel>
    {
        private readonly VRContext _vrContext;

        public GetBookingsHandler(VRContext vrContext)
        {
            _vrContext = vrContext;
        }

        public async Task<BookingViewModel> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
        {
            var booking = await _vrContext.Bookings
                .Include(b => b.Unit)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId);

            if (booking == null)
                throw new ApplicationException("Booking not found");

            var bookingModel = new BookingViewModel
            {
                Id = booking.Id,
                Nights = booking.Nights,
                RentalId = booking.Unit.RentalId,
                Start = booking.Start,
            };

            return bookingModel;
        }
    }
}
