using MediatR;
using System;
using VacationRental.BusinessLogic.Models;

namespace VacationRental.BusinessLogic.Commands.Rentals
{
    public class CreateRentalCommand : IRequest<ResourceIdViewModel>
    {
        public int Units { get; set; }
    }
}
