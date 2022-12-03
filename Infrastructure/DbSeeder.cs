using Application.Interfaces;

namespace Infrastructure
{
    public class DbSeeder : IDbSeeder
    {
        private readonly DatabaseContext _databaseContext;

        public DbSeeder(DatabaseContext context)
        {
            _databaseContext = context;
        }


        public void SeedDevelopment()
        {
            _databaseContext.Database.EnsureDeleted();
            _databaseContext.Database.EnsureCreated();
        }

        public void SeedProduction()
        {
            _databaseContext.Database.EnsureCreated();
        }
    }
}
