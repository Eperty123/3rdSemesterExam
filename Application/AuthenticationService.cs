using Application.DTOs;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;
        private JwtConfig _jwtConfig;

        public AuthenticationService(IUserRepository userRepository, IMapper mapper, JwtConfig jwtConfig)
        {
            if (userRepository == null) throw new ArgumentException("Missing repository");
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtConfig = jwtConfig;
        }

        public AuthenticationService(IUserRepository userRepository, IMapper mapper, IOptions<JwtConfig> jwtConfig)
        {
            if (userRepository == null) throw new ArgumentException("Missing repository");
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtConfig = jwtConfig.Value;
        }

        public TokenDTO Login(LoginUserDTO loginUserDTO)
        {
            var foundUser = _userRepository.ReadUserByUsername(loginUserDTO.Username);
            if (foundUser != null)
            {
                if (!foundUser.Password.VerifyHashedPasswordBCrypt(loginUserDTO.Password))
                    throw new ArgumentException("Wrong password");

                return new TokenDTO { Token = GenerateToken(foundUser), UserId = foundUser.Id, UserType = foundUser.Usertype };
            }

            throw new KeyNotFoundException("User not found.");
        }

        private string GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("username", user.Username), new Claim("role", user.Usertype) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
