using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;
using VacationRental.Database;

namespace VacationRental.BusinessLogic.Queries.Rentals
{
    public class GetRentalsHandler : IRequestHandler<GetRentalsQuery, RentalViewModel>
    {
        private readonly VRContext _vrContext;

        public GetRentalsHandler(VRContext vrContext)
        {
            _vrContext = vrContext;
        }

        public async Task<RentalViewModel> Handle(GetRentalsQuery request, CancellationToken cancellationToken)
        {
            var rental = await _vrContext.Rentals.FirstOrDefaultAsync(b => b.Id == request.RentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            var rentalModel = new RentalViewModel
            { 
               Id = rental.Id,
               Units = rental.Id
            };

            return rentalModel;
        }
    }
}
