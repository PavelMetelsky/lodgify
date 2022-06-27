using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;
using VacationRental.Database;

namespace VacationRental.BusinessLogic.Queries.Calendars
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

            var rental = await _vrContext.Rentals.FirstOrDefaultAsync(b => b.Id == request.RentalId);

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
                    Bookings = new List<CalendarBookingViewModel>()
                };

                foreach (var booking in _vrContext.Bookings)
                {
                    if (booking.RentalId == request.RentalId
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
