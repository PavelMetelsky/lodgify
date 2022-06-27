using MediatR;
using VacationRental.BusinessLogic.Models.Rentals;

namespace VacationRental.BusinessLogic.Queries.Rentals
{
    public class GetRentalsQuery : IRequest<RentalViewModel>
    {
        public int RentalId { get; set; }
    }
}
