using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;
using VacationRental.Database;
using VacationRental.Entities;

namespace VacationRental.BusinessLogic.Commands.Bookings
{
    public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, ResourceIdViewModel>
    {
        private readonly VRContext _vrContext;

        public CreateBookingHandler(VRContext vrContext)
        {
            _vrContext = vrContext;
        }

        public async Task<ResourceIdViewModel> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            if (request.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");

            var rental = await _vrContext.Rentals.FirstOrDefaultAsync(b => b.Id == request.RentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            if (!rental.Active)
                throw new ApplicationException("You can't add rental. Try later");

            for (var i = 0; i < request.Nights; i++)
            {
                var count = 0;
                foreach (var booking in _vrContext.Bookings.Where(b => b.RentalId == request.RentalId))
                {
                    if ((booking.Start <= request.Start.Date && booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays) > request.Start.Date)
                        || (booking.Start < request.Start.AddDays(request.Nights + rental.PreparationTimeInDays) && booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays) >= request.Start.AddDays(request.Nights + rental.PreparationTimeInDays))
                        || (booking.Start > request.Start && booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays) < request.Start.AddDays(request.Nights + rental.PreparationTimeInDays)))
                    {
                        count++;
                    }
                }
                if (count >= rental.Units)
                    throw new ApplicationException("Not available");
            }

            var bookingEntity = new Booking
            {
                Nights = request.Nights,
                RentalId = request.RentalId,
                Start = request.Start.Date,
                Active = true
            };

            await _vrContext.Bookings.AddAsync(bookingEntity, cancellationToken);

            await _vrContext.SaveChangesAsync();

            return new ResourceIdViewModel
            {
                Id = bookingEntity.Id
            };
        }
    }
}
