using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;
using VacationRental.Database;

namespace VacationRental.BusinessLogic.Queries.Books
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
            var booking = await _vrContext.Bookings.FirstOrDefaultAsync(b => b.Id == request.BookingId);
            if (booking == null)
                throw new ApplicationException("Booking not found");

            var bookingModel = new BookingViewModel { 
                Id = booking.Id,
                Nights = booking.Nights,
                RentalId = booking.RentalId,
                Start = booking.Start,
            };

            return bookingModel;
        }
    }
}
