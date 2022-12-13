using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<TokenDTO> Login(LoginUserDTO loginUserDTO)
        {
            var token = _authService.Login(loginUserDTO);
            if (token != null) return token;

            return BadRequest("User with the username not found.");
        }
    }
}
