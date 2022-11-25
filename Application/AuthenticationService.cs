using Application.DTOs;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            if (userRepository == null) throw new ArgumentException("Missing repository");
            _userRepository = userRepository;
        }

        public string Login(LoginUserDTO loginUserDTO)
        {
            throw new NotImplementedException();
        }

        public string Register(RegisterUserDTO registerUserDTO)
        {
            throw new NotImplementedException();
        }
    }
}
