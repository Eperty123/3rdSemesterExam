using Application.DTOs;
using Application.Interfaces;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
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

        [HttpPut]
        public ActionResult<User> UpdateUser(UpdateUserDTO dto)
        {
            try
            {
                var result = _userService.UpdateUser(dto.Id, dto);
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

        [HttpGet]
        public void RebuildDB()
        {
            _userService.RebuildDB();
        }
    }
}