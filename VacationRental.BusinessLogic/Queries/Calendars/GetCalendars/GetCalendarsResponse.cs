using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;
using VacationRental.Database;

namespace VacationRental.BusinessLogic.Queries.Calendars.GetCalendars
{
    public class GetCalendarsResponse
    {
        public CalendarViewModel Calendar { get; set; }
    }
}
