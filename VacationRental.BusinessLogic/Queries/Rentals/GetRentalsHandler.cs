using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models.Rentals;
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

            return new RentalViewModel
            { 
               Id = rental.Id,
               //Units = rental.Units,
               PreparationTimeInDays = rental.PreparationTimeInDays
            };
        }
    }
}
