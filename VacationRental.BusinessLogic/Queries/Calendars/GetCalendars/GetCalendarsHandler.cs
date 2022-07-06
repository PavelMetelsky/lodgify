using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;
using VacationRental.Database;

namespace VacationRental.BusinessLogic.Queries.Calendars.GetCalendars
{
    public class GetCalendarsHandler : IRequestHandler<GetCalendarsQuery, CalendarViewModel>
    {
        private readonly VRContext _vrContext;

        public GetCalendarsHandler(VRContext vrContext)
        {
            _vrContext = vrContext;
        }

        public async Task<CalendarViewModel> Handle(GetCalendarsQuery request, CancellationToken cancellationToken)
        {
            if (request.Nights < 0)
                throw new ApplicationException("Nights must be positive");

            var rental = await _vrContext.Rentals
                 .Include(r => r.Units)
                    .ThenInclude(u => u.Bookings
                            .Where(b => b.Start < request.End.AddDays(b.Unit.Rental.PreparationTimeInDays)
                                && request.Start < b.End.AddDays(b.Unit.Rental.PreparationTimeInDays)))
                 .FirstOrDefaultAsync(b => b.Id == request.RentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            var result = new CalendarViewModel
            {
                RentalId = request.RentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var i = 0; i < request.Nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = request.Start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                };

                foreach (var booking in rental.Units.SelectMany(b => b.Bookings))
                {
                    if (booking.Start <= date.Date && date.Date <= booking.End)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = booking.UnitId });
                    }
                    if (booking.End < date.Date && date.Date <= booking.End.AddDays(rental.PreparationTimeInDays))
                    {
                        date.PreparationTimes.Add(new CalendarPreparationTimeViewModel { Unit = booking.UnitId });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
