using Application;
using Application.Interfaces;
using Application.Validators;
using Domain;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XunitTest
{
    public class BookingServiceTest
    {
        private BookingValidator _bookingValidator;

        public BookingServiceTest()
        {
            _bookingValidator = new BookingValidator();
        }

        //Test 3.1
        [Fact]
        public void CreateBookingServiceWithValidRepository()
        {
            //Arrange
            Mock<IBookingRepository> mockRepository = new Mock<IBookingRepository>();
            IBookingRepository repository = mockRepository.Object;

            //Act
            IBookingService bookingService = new BookingService(repository, _bookingValidator);

            //Assert
            Assert.NotNull(bookingService);
            Assert.True(bookingService is BookingService);
        }

        //Test 3.2
        [Fact]
        public void CreateBookingServiceWithInvalidRepository()
        {
            //Arrange
            IBookingService bookingService = null;

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => bookingService = new BookingService(null, _bookingValidator));

            Assert.Equal("Missing repository", ex.Message);
            Assert.Null(bookingService);
        }

        //Test 3.3
        [Fact]
        public void CreateValidNewBooking()
        {
            //Arrange
            Mock<IBookingRepository> mockRepository = new Mock<IBookingRepository>();
            IBookingRepository repository = mockRepository.Object;

            Booking validBooking = new Booking() { CoachId = 1, Date = DateTime.Now.AddHours(1)};

            mockRepository.Setup(x => x.CreateBooking(validBooking)).Returns(validBooking);

            IBookingService service = new BookingService(repository, _bookingValidator);

            //Act
            Booking result = service.CreateBooking(validBooking);

            //Assert
            Assert.Equal(validBooking, result);
            mockRepository.Verify(x => x.CreateBooking(validBooking), Times.Once);
        }

        //Test 3.4
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, -1)]
        public void CreateInvalidNewBooking(int coachId, int hoursToAdd)
        {
            //Arrange
            Mock<IBookingRepository> mockRepository = new Mock<IBookingRepository>();
            IBookingRepository repository = mockRepository.Object;

            Booking invalidBooking = new Booking() { CoachId = coachId, Date = DateTime.Now.AddHours(hoursToAdd) };

            mockRepository.Setup(x => x.CreateBooking(invalidBooking)).Returns(invalidBooking);

            IBookingService service = new BookingService(repository, _bookingValidator);

            //Act + Assert
            Assert.Throws<ValidationException>(() => service.CreateBooking(invalidBooking));

            mockRepository.Verify(x => x.CreateBooking(invalidBooking), Times.Never);
        }
    }
}
