using Application.Interfaces;

namespace Infrastructure
{
    public class DbSeeder : IDbSeeder
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IUserService _userService;

        public DbSeeder(DatabaseContext context, IUserService userService)
        {
            _databaseContext = context;
            _userService = userService;
        }


        public void SeedDevelopment()
        {
            _databaseContext.Database.EnsureDeleted();
            _databaseContext.Database.EnsureCreated();

            _userService.CreateUser(new Application.DTOs.RegisterUserDTO
            {
                Email = "carlo@easv.dk",
                Username = "carlo",
                Password = "dev",
                Usertype = "Coach",
            });

            _userService.CreateUser(new Application.DTOs.RegisterUserDTO
            {
                Email = "annso@easv.dk",
                Username = "annso",
                Password = "dev",
                Usertype = "Coach",
            });

            _userService.CreateUser(new Application.DTOs.RegisterUserDTO
            {
                Email = "client@easv.dk",
                Username = "client",
                Password = "dev",
                Usertype = "Client",
            });
        }

        public void SeedProduction()
        {
            _databaseContext.Database.EnsureCreated();
        }
    }
}
