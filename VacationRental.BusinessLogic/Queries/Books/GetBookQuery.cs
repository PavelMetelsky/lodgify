using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.BusinessLogic.Models;

namespace VacationRental.BusinessLogic.Queries.Books
{
    public class GetBookQuery : IRequest<BookingViewModel>
    {
        [FromRoute]
        public int BookingId { get; set; }
}
}
