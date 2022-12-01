using Application.DTOs;
using Application.Helpers;
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
        private IValidator<LoginUserDTO> _loginUserValidator;

        public UserService(IUserRepository userRepository, IMapper mapper, IValidator<RegisterUserDTO> registerUserValidator, IValidator<LoginUserDTO> loginUserValidator)
        {
            if (userRepository == null)
                throw new ArgumentException("Missing repository");

            _userRepository = userRepository;
            _mapper = mapper;
            _registerUserValidator = registerUserValidator;
            _loginUserValidator = loginUserValidator;
        }

        public User CreateUser(RegisterUserDTO dto)
        {
            var validation = _registerUserValidator.Validate(dto);

            if (!validation.IsValid)
                throw new ValidationException(validation.ToString());

            // Hash the password to prevent plain text. Not super secure but should
            // fullfill the basic security requirement.
            dto.Password = dto.Password.HashPasswordBCrypt();
            var userType = dto.Usertype;
            return _userRepository.CreateUser(userType == "Client" ? _mapper.Map<Client>(dto) : _mapper.Map<Coach>(dto));
        }

        public User DeleteUser(int id)
        {
            if (id <= 0) throw new ArgumentException("The id cannot be 0 or lower!");

            return _userRepository.DeleteUser(id);
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUser(int id)
        {
            if (id <= 0) throw new ArgumentException("The id cannot be 0 or lower!");

            return _userRepository.ReadUserById(id);
        }

        public User UpdateUser(int id, UpdateUserDTO dto)
        {
            if (id <= 0) throw new ArgumentException("The id cannot be 0 or lower!");
            return _userRepository.UpdateUser(id, _mapper.Map<User>(dto), dto.OldPassword);
        }

        public void RebuildDB()
        {
            _userRepository.RebuildDB();
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
    }
}
