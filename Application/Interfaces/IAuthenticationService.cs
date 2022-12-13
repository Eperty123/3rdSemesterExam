using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAuthenticationService
    {
        TokenDTO Register(RegisterUserDTO registerUserDTO);
        TokenDTO Login(LoginUserDTO loginUserDTO);
    }
}
