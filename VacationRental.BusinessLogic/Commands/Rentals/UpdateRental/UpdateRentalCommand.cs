using MediatR;
using VacationRental.BusinessLogic.Models;

namespace VacationRental.BusinessLogic.Commands.Rentals.CreateRental
{
    public class UpdateRentalCommand : IRequest<ResourceIdViewModel>
    {
        public int RentalId { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
