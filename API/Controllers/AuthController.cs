using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public ActionResult<TokenDTO> Login(LoginUserDTO loginUserDTO)
        {
            var foundUser = _userService.GetUserByUsername(loginUserDTO);
            if (foundUser != null)
            {
                var token = new TokenDTO { Token = foundUser.Password, UserId = foundUser.Id, UserType = foundUser.Usertype };
                return Ok(token);
            }

            return BadRequest("User with the username not found.");
        }
    }
}
