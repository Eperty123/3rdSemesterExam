using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            if (userRepository == null)
                throw new ArgumentException("Missing repository");
            _userRepository = userRepository;
        }

        public User CreateUser(string username, string password, string email, string usertype)
        {
            //var dto = new RegisterUserDTO() { Usertype= usertype, Email = email, Password = password, Username = username };
            //UserValidator validator = new UserValidator();
            //if (!validator.Validate(dto).IsValid)
            //    throw new ArgumentException();
            return _userRepository.CreateUser(username, password, email, usertype);
        }

        public User DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public User UpdateUser(int id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
