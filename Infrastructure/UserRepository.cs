using Application.Interfaces;
using Application.DTOs;
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
            throw new NotImplementedException();
        }

        public User ReadUserById(int id)
        {
            return _context.UserTable.FirstOrDefault(u => u.Id == id) ?? throw new KeyNotFoundException("There was no user with id " + id);
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
            return _context.UserTable.ToList();
        }

        public List<Coach> ReadAllCoaches()
        {
            return _context.CoachTable.ToList();
        }

        public List<Client> ReadAllClients()
        {
            return _context.ClientTable.ToList();
        }

        public User UpdateUser(int id, User user, string oldPassword)
        {
            var foundUser = _context.UserTable.FirstOrDefault(x => x.Id == id);
            if (foundUser != null)
            {
                if (!foundUser.Password.VerifyHashedPasswordBCrypt(oldPassword))
                    throw new ArgumentException("Entered password does not match");

                // Database user must always have their password hashed!
                foundUser.Password = user.Password.HashPasswordBCrypt();
                _context.UserTable.Update(foundUser);
                _context.SaveChanges();

                return foundUser;
            }

            return null;
        }

        public Coach ReadCoachById(int id)
        {
            return _context.CoachTable.FirstOrDefault(u => u.Id == id) ?? throw new KeyNotFoundException("There was no user with id " + id);
        }
    }
}