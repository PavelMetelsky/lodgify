namespace VacationRental.Entities
{
    public class Unit
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public int RentalId { get; set; }
        public Rental Rental { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
