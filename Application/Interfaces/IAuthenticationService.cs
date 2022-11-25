using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAuthenticationService
    {
        string Register(RegisterUserDTO registerUserDTO);
        string Login(LoginUserDTO loginUserDTO);
    }
}
