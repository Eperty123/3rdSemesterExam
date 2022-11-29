using Application.Helpers;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private DatabaseContext _context;

        public UserRepository(DatabaseContext databaseContext)
        {
            _context = databaseContext;

        }

        public User CreateUser(User user)
        {
            try
            {
                ReadUserByEmail(user.Email);
                throw new Exception("This email is already in use");
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    ReadUserByUsername(user.Username);
                    throw new Exception("This username is already in use");
                }
                catch (KeyNotFoundException)
                {
                    _context.UserTable.Add(user);
                    _context.SaveChanges();
                    return user;
                }
            }
        }

        public User DeleteUser(int id)
        {
            var foundUser = _context.UserTable.FirstOrDefault(x => x.Id == id);
            if (foundUser != null)
            {
                _context.UserTable.Remove(foundUser);
                _context.SaveChanges();

                return foundUser;
            }

            return null;
        }

        public User ReadUserById(int id)
        {
            throw new NotImplementedException();
        }

        public User ReadUserByEmail(string email)
        {
            return _context.UserTable.FirstOrDefault(u => u.Email == email) ?? throw new KeyNotFoundException("There was no user with email " + email);
        }
        public User ReadUserByUsername(string username)
        {
            return _context.UserTable.FirstOrDefault(u => u.Username == username) ?? throw new KeyNotFoundException("There was no user with username " + username);
        }

        public List<User> ReadAllUsers()
        {
            throw new NotImplementedException();
        }

        public User UpdateUser(int id, User user)
        {
            var foundUser = _context.UserTable.FirstOrDefault(x => x.Id == id);
            if (foundUser != null)
            {
                //foundUser.Username = user.Username;
                // Database user must always have their password hashed!
                foundUser.Password = user.Password.HashPasswordBCrypt();
                //foundUser.Email = user.Email;
                //foundUser.Bookings = user.Bookings;
                _context.UserTable.Update(foundUser);
                _context.SaveChanges();

                return foundUser;
            }

            return null;
        }
        public void RebuildDB()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
    }
}