using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.BusinessLogic.Models;

namespace VacationRental.BusinessLogic.Queries.Books
{
    public class GetBookResponse
    {
        public BookingViewModel Book { get; set; }
    }
}
