using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private DbContextOptions<DbContext> _opts;

        public UserRepository() 
        {
            _opts= new DbContextOptionsBuilder<DbContext>()
                .UseSqlite("Data source=../API/db.db").Options; ;
        }
        public User CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public User DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> ReadAllUsers()
        {
            throw new NotImplementedException();
        }

        public User UpdateUser(int id, User user)
        {
            throw new NotImplementedException();
        }
    }
}