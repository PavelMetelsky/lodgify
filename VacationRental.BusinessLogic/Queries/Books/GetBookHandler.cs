using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;

namespace VacationRental.BusinessLogic.Queries.Books
{
    public class GetBookHandler : IRequestHandler<GetBookQuery, BookingViewModel>
    {
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public GetBookHandler(IDictionary<int, BookingViewModel> bookings)
        {
            _bookings = bookings;
        }

        public Task<BookingViewModel> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            if (!_bookings.ContainsKey(request.BookingId))
                throw new ApplicationException("Booking not found");

            return Task.FromResult(_bookings[key: request.BookingId]);
        }
    }
}
