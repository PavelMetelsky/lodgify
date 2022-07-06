using MediatR;
using System;
using VacationRental.BusinessLogic.Models;

namespace VacationRental.BusinessLogic.Commands.Rentals.CreateRental
{
    public class CreateRentalCommand : IRequest<ResourceIdViewModel>
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
