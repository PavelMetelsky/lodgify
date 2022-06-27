using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;
using VacationRental.Database;

namespace VacationRental.BusinessLogic.Commands.Rentals.CreateRental
{
    public class UpdateRentalHandler : IRequestHandler<UpdateRentalCommand, ResourceIdViewModel>
    {
        private readonly VRContext _vrContext;

        public UpdateRentalHandler(VRContext vrContext)
        {
            _vrContext = vrContext;
        }

        public async Task<ResourceIdViewModel> Handle(UpdateRentalCommand request, CancellationToken cancellationToken)
        {

            var rentalEntity = await _vrContext.Rentals.FirstOrDefaultAsync(b => b.Id == request.RentalId);

            if (rentalEntity == null)
                throw new ApplicationException("Rental not found");

            rentalEntity.Active = false;
            await _vrContext.SaveChangesAsync();

            var bookingsCount = await _vrContext.Bookings.CountAsync(x => x.RentalId == request.RentalId);
            if (bookingsCount > request.Units)
            {
                throw new ApplicationException("You cann't specify less units than alreafy booked");
            }

            if (request.PreparationTimeInDays > rentalEntity.PreparationTimeInDays)
            {
                var bookings = await _vrContext.Bookings.Where(x => x.RentalId == request.RentalId).ToListAsync();

                foreach (var booking in bookings)
                {
                    var endBookingDate = booking.Start.AddDays(booking.Nights + request.PreparationTimeInDays);

                    if (bookings.Exists(x => x.Start <= endBookingDate))
                    {
                        throw new ApplicationException("PreparationTime can't be updated due to overlapping between existing bookings");
                    }
                }
            }

            rentalEntity.Units = request.Units;
            rentalEntity.PreparationTimeInDays = request.PreparationTimeInDays;
            rentalEntity.Active = true;

            await _vrContext.SaveChangesAsync();

            return new ResourceIdViewModel
            {
                Id = rentalEntity.Id
            };
        }
    }
}
