using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using FluentValidation;
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
        private IMapper _mapper;
        private IValidator<RegisterUserDTO> _registerUserValidator;

        public UserService(IUserRepository userRepository, IMapper mapper, IValidator<RegisterUserDTO> registerUserValidator)
        {
            if (userRepository == null)
                throw new ArgumentException("Missing repository");

            _userRepository = userRepository;
            _mapper = mapper;
            _registerUserValidator = registerUserValidator;
        }

        public User CreateUser(RegisterUserDTO dto)
        {
            var validation = _registerUserValidator.Validate(dto);

            if (!validation.IsValid)
                throw new ValidationException(validation.ToString());

            return _userRepository.CreateUser(_mapper.Map<User>(dto));
        }

        public User DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.ReadAllUsers();
        }

        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public User UpdateUser(int id, User user)
        {
            throw new NotImplementedException();
        }

        public User GetUserByUsername(LoginUserDTO dto)
        {
            try
            {
                var foundUser = _userRepository.ReadUserByUsername(dto.Username);

                if (!foundUser.Password.VerifyHashedPasswordBCrypt(dto.Password))
                    throw new ValidationException("Wrong login credentials");

                return foundUser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Coach GetCoach(int id)
        {
            if (id <= 0) throw new ArgumentException("The id cannot be 0 or lower!");

            return _userRepository.ReadCoachById(id);
        }
    }
}
