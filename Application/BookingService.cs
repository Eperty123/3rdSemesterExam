using Application.Interfaces;
using Application.Validators;
using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class BookingService : IBookingService
    {
        private IBookingRepository _bookingRepository;
        private BookingValidator _bookingValidator;
        public BookingService(IBookingRepository repository, BookingValidator bookingValidator)
        {
            if (repository == null)
                throw new ArgumentException("Missing repository");
            _bookingRepository = repository;
            _bookingValidator = bookingValidator;
        }
        public Booking CreateBooking(Booking booking)
        {
            var validation = _bookingValidator.Validate(booking);
            if (!validation.IsValid)
                throw new ValidationException(validation.ToString());
            return _bookingRepository.CreateBooking(booking);
        }

        public Booking DeleteBooking(int id)
        {
            return _bookingRepository.DeleteBooking(id);
        }
    }
}
