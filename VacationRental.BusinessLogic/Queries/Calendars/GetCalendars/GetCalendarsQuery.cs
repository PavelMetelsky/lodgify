using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.BusinessLogic.Models;

namespace VacationRental.BusinessLogic.Queries.Calendars.GetCalendars
{
    public class GetCalendarsQuery : IRequest<CalendarViewModel>
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
