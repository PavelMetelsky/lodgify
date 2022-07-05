namespace VacationRental.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public bool Active { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        public DateTime End => Start.AddDays(Nights);
    }
}
