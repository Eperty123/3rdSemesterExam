using Application.Interfaces;
using Domain;

namespace Infrastructure
{
    public class DbSeeder : IDbSeeder
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;

        public DbSeeder(DatabaseContext context, IUserService userService, IBookingService bookingService)
        {
            _databaseContext = context;
            _userService = userService;
            _bookingService = bookingService;
        }


        public void SeedDevelopment()
        {
            _databaseContext.Database.EnsureDeleted();
            _databaseContext.Database.EnsureCreated();

            var coach1 = _userService.CreateUser(new Application.DTOs.RegisterUserDTO
            {
                Email = "carlo@easv.dk",
                Username = "carlo",
                Password = "dev",
                Usertype = "Coach",
                Description = "I coach in pretty much every game out there."
            });

            var coach2 = _userService.CreateUser(new Application.DTOs.RegisterUserDTO
            {
                Email = "annso@easv.dk",
                Username = "annso",
                Password = "dev",
                Usertype = "Coach",
                Description = "I coach Heart Stone and League of Legends."
            });

            var client = _userService.CreateUser(new Application.DTOs.RegisterUserDTO
            {
                Email = "client@easv.dk",
                Username = "client",
                Password = "dev",
                Usertype = "Client",
                Description = "I'm just your typical client."
            });

            _bookingService.CreateBooking(new Booking
            {
                Client = (Client)client,
                Coach = (Coach)coach1,
                ClientId = client.Id,
                CoachId = coach1.Id,
                Date = DateTime.Now,
            });

            _bookingService.CreateBooking(new Booking
            {
                Client = (Client)client,
                Coach = (Coach)coach2,
                ClientId = client.Id,
                CoachId = coach2.Id,
                Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            });

            _bookingService.ChangeAvailableTimes(new Application.DTOs.AvailableTimesDTO
            {
                CoachId = coach1.Id,
                StartTime = "08:00",
                EndTime = "16:00",
            });

            _bookingService.ChangeAvailableTimes(new Application.DTOs.AvailableTimesDTO
            {
                CoachId = coach2.Id,
                StartTime = "08:00",
                EndTime = "18:00",
            });
        }

        public void SeedProduction()
{
    _databaseContext.Database.EnsureCreated();
}
    }
}
