﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;
using VacationRental.Database;
using VacationRental.Entities;

namespace VacationRental.BusinessLogic.Commands.Rentals.CreateRental
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
                Units = new List<Entities.Unit>(),
                PreparationTimeInDays = request.PreparationTimeInDays,
                Active = true
            };

            for (var i = 0; i < request.Units; i++)
            {
                rentalEntity.Units.Add(new Entities.Unit { Active = true});
            }

            await _vrContext.Rentals.AddAsync(rentalEntity, cancellationToken);
            await _vrContext.SaveChangesAsync();

            return new ResourceIdViewModel
            {
                Id = rentalEntity.Id
            };
        }
    }
}
