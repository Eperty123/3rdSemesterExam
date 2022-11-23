namespace Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public int ClientId { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public bool IsBooked { get; set; }
        public List<User> users { get; set; }

        //public User Coach { get; set; }
        //public User? Client { get; set; }
    }
}