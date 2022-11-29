namespace Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public int CoachId { get; set; }
        public Client Client { get; set; }
        public Coach Coach { get; set; }
    }
}