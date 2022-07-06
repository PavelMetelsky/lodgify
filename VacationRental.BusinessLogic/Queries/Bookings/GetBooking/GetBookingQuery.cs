using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.BusinessLogic.Models.Bookings;

namespace VacationRental.BusinessLogic.Queries.Bookings.GetBooking
{
    public class GetBookingsQuery : IRequest<BookingViewModel>
    {
        public int BookingId { get; set; }
    }
}
