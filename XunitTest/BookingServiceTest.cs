using Application;
using Application.DTOs;
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
        private Coach[] _fakeCoachRepo = new Coach[]
        {
            new Coach() { Id = 1, Email = "test@test.com", Username = "carlo" },
            new Coach() { Id = 2, Email = "test1@test.com", Username = "annso" },
        };

        public BookingServiceTest()
        {
            _bookingValidator = new BookingValidator();
        }

        //Test 3.1 - Successfully create BookingService with valid repository
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

        //Test 3.2 - Throw ArgumentException when trying to create BookingService with invalid repository
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

        // 3.3 - Returns a valid and updated Coach with updated start and end time for booking
        [Theory]
        [InlineData(1, "08:00", "16:00")]
        [InlineData(2, "16:00", "22:00")]

        public void ValidChangeAvailableTime(int coachId, string startTime, string endTime)
        {
            //Arrange
            Mock<IBookingRepository> mockRepository = new Mock<IBookingRepository>();
            IBookingRepository repository = mockRepository.Object;

            AvailableTimesDTO availableTimesDTO = new AvailableTimesDTO { CoachId = coachId, StartTime = startTime, EndTime = endTime };
            Coach desiredCoach = _fakeCoachRepo.FirstOrDefault(x => x.Id == coachId);

            mockRepository.Setup(r => r.ChangeAvailableTimes(availableTimesDTO)).Returns(desiredCoach).Callback(new Action(() =>
            {
                desiredCoach.StartTime = TimeOnly.Parse(startTime);
                desiredCoach.EndTime = TimeOnly.Parse(endTime);
            }));

            IBookingService service = new BookingService(repository, _bookingValidator);

            // Act
            var expected = service.ChangeAvailableTimes(availableTimesDTO);

            // Assert
            Assert.NotNull(expected);
            Assert.Equal(startTime, expected.StartTime.ToString("HH:mm"));
            Assert.Equal(endTime, expected.EndTime.ToString("HH:mm"));
            mockRepository.Verify(r => r.ChangeAvailableTimes(availableTimesDTO), Times.Once);
        }

        // 3.4 - Returns a NullReferenceException from trying to find an non-existent Coach
        [Theory]
        [InlineData(0, "08:00", "16:00")]
        [InlineData(-1, "16:00", "22:00")]

        public void InvalidChangeAvailableTime(int coachId, string startTime, string endTime)
        {
            //Arrange
            Mock<IBookingRepository> mockRepository = new Mock<IBookingRepository>();
            IBookingRepository repository = mockRepository.Object;

            AvailableTimesDTO availableTimesDTO = new AvailableTimesDTO { CoachId = coachId, StartTime = startTime, EndTime = endTime };
            Coach desiredCoach = _fakeCoachRepo.FirstOrDefault(x => x.Id == coachId);

            mockRepository.Setup(r => r.ChangeAvailableTimes(availableTimesDTO)).Returns(desiredCoach).Callback(new Action(() =>
            {
                desiredCoach.StartTime = TimeOnly.Parse(startTime);
                desiredCoach.EndTime = TimeOnly.Parse(endTime);
            }));

            IBookingService service = new BookingService(repository, _bookingValidator);

            // Act + assert
            Assert.Throws<NullReferenceException>(() => service.ChangeAvailableTimes(availableTimesDTO));
            mockRepository.Verify(r => r.ChangeAvailableTimes(availableTimesDTO), Times.Once);
        }

        //Test 5.1 - Create new available booking with valid inputs
        [Fact]
        public void CreateValidNewBooking()
        {
            //Arrange
            Mock<IBookingRepository> mockRepository = new Mock<IBookingRepository>();
            IBookingRepository repository = mockRepository.Object;

            Booking validBooking = new Booking() { CoachId = 1, Date = DateTime.Now.AddHours(1) };

            mockRepository.Setup(x => x.CreateBooking(validBooking)).Returns(validBooking);

            IBookingService service = new BookingService(repository, _bookingValidator);

            //Act
            Booking result = service.CreateBooking(validBooking);

            //Assert
            Assert.Equal(validBooking, result);
            mockRepository.Verify(x => x.CreateBooking(validBooking), Times.Once);
        }

        //Test 5.2 - Throw exception when trying to create booking with invalid inputs
        [Theory]
        [InlineData(0, 12, 12, 2022, typeof(ValidationException))]
        [InlineData(1, 12, 5, 2022, typeof(ValidationException))]
        public void CreateInvalidNewBooking(int coachId, int month, int day, int year, Type exceptionType)
        {
            //Arrange
            Mock<IBookingRepository> mockRepository = new Mock<IBookingRepository>();
            IBookingRepository repository = mockRepository.Object;

            Booking invalidBooking = new Booking() { CoachId = coachId, Date = new DateTime(year, month, day) };

            mockRepository.Setup(x => x.CreateBooking(invalidBooking)).Returns(invalidBooking);

            IBookingService service = new BookingService(repository, _bookingValidator);

            //Act + Assert
            var exception = Assert.Throws(exceptionType, () => service.CreateBooking(invalidBooking));
            Assert.IsType(exceptionType, exception);

            mockRepository.Verify(x => x.CreateBooking(invalidBooking), Times.Never);
        }
    }
}
