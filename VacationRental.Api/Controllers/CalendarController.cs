using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.BusinessLogic.Models;
using VacationRental.BusinessLogic.Queries.Calendars;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<CalendarViewModel> Get([FromQuery] GetCalendarsQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
