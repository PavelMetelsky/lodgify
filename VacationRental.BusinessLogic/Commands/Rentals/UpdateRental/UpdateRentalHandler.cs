using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;
using VacationRental.Database;
using VacationRental.Entities;

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
            var rental = await _vrContext.Rentals
                .Include(r => r.Units)
                    .ThenInclude(u => u.Bookings
                        .Where(b => DateTime.UtcNow < b.End.AddDays(b.Unit.Rental.PreparationTimeInDays)))
                .FirstOrDefaultAsync(b => b.Id == request.RentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            await DeactivateAndValidateRental(rental, request);
            await UpdateRental(rental, request);

            return new ResourceIdViewModel
            {
                Id = rental.Id
            };
        }

        private async Task DeactivateAndValidateRental(Rental rental, UpdateRentalCommand request)
        {
            rental.Active = false;
            await _vrContext.SaveChangesAsync();

            if (rental.Units.Count(u => u.Bookings.Any()) >= request.Units)
            {
                throw new ApplicationException("You cann't specify less units than alreafy booked");
            }

            if (request.PreparationTimeInDays > rental.PreparationTimeInDays)
            {
                foreach (var unit in rental.Units)
                {
                    var bookings = unit.Bookings.OrderBy(b => b.Start).ToArray();

                    for (var i = 1; i < bookings.Length; i++)
                    {
                        if (bookings[i - 1].End.AddDays(request.PreparationTimeInDays) > bookings[i].Start)
                        {
                            throw new ApplicationException("PreparationTime can't be updated due to overlapping between existing bookings");
                        }
                    }
                }
            }
        }

        private async Task UpdateRental(Rental rental, UpdateRentalCommand request)
        {
            var unitDelta = rental.Units.Count - request.Units;

            if (unitDelta < 0)
            {
                for (var i = 0; i < -unitDelta; i++)
                {
                    rental.Units.Add(new Entities.Unit { Active = true });
                }
            }
            else
            {
                foreach (var unit in rental.Units.Where(u => !u.Bookings.Any()).Take(unitDelta).ToList())
                {
                    rental.Units.Remove(unit);
                }
            }

            rental.PreparationTimeInDays = request.PreparationTimeInDays;
            rental.Active = true;

            await _vrContext.SaveChangesAsync();
        }
    }
}
