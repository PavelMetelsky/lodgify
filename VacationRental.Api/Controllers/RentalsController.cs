using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.BusinessLogic.Commands.Rentals;
using VacationRental.BusinessLogic.Models;
using VacationRental.BusinessLogic.Queries.Rentals;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RentalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{rentalId}")]
        public async Task<RentalViewModel> Get(int rentalId)
        {
            return await _mediator.Send(new GetRentalsQuery { RentalId = rentalId });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRentalCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
