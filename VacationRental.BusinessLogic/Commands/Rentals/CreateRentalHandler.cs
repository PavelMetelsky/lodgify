using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;
using VacationRental.Database;
using VacationRental.Entities;

namespace VacationRental.BusinessLogic.Commands.Rentals
{
    public class CreateRentalHandler : IRequestHandler<CreateRentalCommand, ResourceIdViewModel>
    {
        private readonly VRContext _vrContext;

        public CreateRentalHandler(VRContext vrContext)
        {
            _vrContext = vrContext;
        }

        public async Task<ResourceIdViewModel> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            var rentalEntity = new Rental
            {
                Units = request.Units
            };

            await _vrContext.Rentals.AddAsync(rentalEntity, cancellationToken);
            await _vrContext.SaveChangesAsync();

            return new ResourceIdViewModel
            {
                Id = rentalEntity.Id
            };
        }
    }
}
