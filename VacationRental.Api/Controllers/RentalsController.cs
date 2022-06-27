using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.BusinessLogic.Commands.Rentals.CreateRental;
using VacationRental.BusinessLogic.Models.Rentals;
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

        [HttpGet("{rentalId:int}")]
        public async Task<RentalViewModel> GetRental(int rentalId)
        {
            return await _mediator.Send(new GetRentalsQuery { RentalId = rentalId });
        }

        [HttpPost]
        public async Task<IActionResult> AddRental([FromBody] CreateRentalCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{rentalId:int}")]
        public async Task<IActionResult> UpdateRental([FromRoute] int rentalId, [FromBody] RentalModel rentalModel)
        {
            return Ok(await _mediator.Send(new UpdateRentalCommand
            {
                RentalId = rentalId,
                Units = rentalModel.Units,
                PreparationTimeInDays = rentalModel.PreparationTimeInDays
            }));
        }
    }
}
