using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        [HttpPut]
        public ActionResult<Coach> ChangeAvailableTimes(AvailableTimesDTO dto)
        {
            try
            {
                var result = _bookingService.ChangeAvailableTimes(dto);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult<Booking> CreateBooking(Booking booking)
        {
            try
            {
                var result = _bookingService.CreateBooking(booking);
                return Created("", result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult<Booking> DeleteBooking(int id)
        {
            try
            {
                return Ok(_bookingService.DeleteBooking(id));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound("No booking found with ID " + id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Booking> GetBooking(int id)
        {
            try
            {
                return Ok(_bookingService.GetBooking(id));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound("No booking found with ID " + id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
    }
}
