using MediatR;
using System;
using VacationRental.BusinessLogic.Models;

namespace VacationRental.BusinessLogic.Commands.Bookings.CreateBooking
{
    public class CreateBookingCommand : IRequest<ResourceIdViewModel>
    {
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        public int Nights { get; set; }

        public DateTime End => Start.AddDays(Nights);
    }
}
