namespace Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public int ClientId { get; set; }
        public DateTime Date { get; set; }
        public bool IsBooked { get; set; }
        public List<User> Participants { get; set; }
    }
}