using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.BusinessLogic.Commands.Bookings;
using VacationRental.BusinessLogic.Models;
using VacationRental.BusinessLogic.Queries.Books;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{bookingId}")]
        public async Task<BookingViewModel> Get(int bookingId)
        {
            return await _mediator.Send(new GetBookingsQuery { BookingId= bookingId });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateBookingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
