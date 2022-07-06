using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;
using VacationRental.Database;
using VacationRental.Entities;
namespace VacationRental.BusinessLogic.Commands.Bookings.CreateBooking
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
                throw new ApplicationException("Nights must be positive");

            var rental = await _vrContext.Rentals
                .Include(r => r.Units)
                    .ThenInclude(u => u.Bookings
                            .Where(b => b.Start < request.End.AddDays(b.Unit.Rental.PreparationTimeInDays)
                                && request.Start < b.End.AddDays(b.Unit.Rental.PreparationTimeInDays)))
                .FirstOrDefaultAsync(b => b.Id == request.RentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            if (!rental.Active)
                throw new ApplicationException("You can't add booking. Try later");

            var availableUnit = rental.Units.FirstOrDefault(u => !u.Bookings.Any());

            if (availableUnit == null)
                throw new ApplicationException("Not available units");

            var bookingEntity = new Booking
            {
                Start = request.Start.Date,
                End = request.Start.Date.AddDays(request.Nights),
                Active = true,
                Unit = availableUnit
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
