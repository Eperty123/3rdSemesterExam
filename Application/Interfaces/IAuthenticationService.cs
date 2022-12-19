using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAuthenticationService
    {
        TokenDTO Login(LoginUserDTO loginUserDTO);
    }
}
