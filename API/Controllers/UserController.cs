using Application.DTOs;
using Application.Interfaces;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                return Ok(_userService.GetAllUsers());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("coaches")]
        public ActionResult<IEnumerable<User>> GetAllCoaches()
        {
            try
            {
                return Ok(_userService.GetAllCoaches());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("clients")]
        public ActionResult<IEnumerable<User>> GetAllClients()
        {
            try
            {
                return Ok(_userService.GetAllClients());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            try
            {
                return Ok(_userService.GetUser(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [AllowAnonymous]
        [HttpGet("coach/{id}")]
        public ActionResult<Coach> GetCoachById(int id)
        {
            try
            {
                return Ok(_userService.GetCoach(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}