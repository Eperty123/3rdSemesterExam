using Application.DTOs;
using Application.Interfaces;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private IUserService _userService;

        public RegisterController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpPost]
        public ActionResult<User> CreateUser(RegisterUserDTO dto)
        {
            try
            {
                var result = _userService.CreateUser(dto);
                return Created("", result);
            }
            catch (ValidationException v)
            {
                return BadRequest(v.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}