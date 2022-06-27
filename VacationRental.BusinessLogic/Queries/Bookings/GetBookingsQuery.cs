using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.BusinessLogic.Models;

namespace VacationRental.BusinessLogic.Queries.Books
{
    public class GetBookingsQuery : IRequest<BookingViewModel>
    {
        public int BookingId { get; set; }
    }
}
