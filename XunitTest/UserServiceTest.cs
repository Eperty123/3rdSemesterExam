using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using FluentValidation;
using Moq;

namespace XunitTest
{
    public class UserServiceTest
    {
        //Test 1.1
        [Fact]
        public void CreateUserServiceWithValidRepository()
        {
            //Arrange
            Mock<IUserRepository> mockRepository = new Mock<IUserRepository>();
            IUserRepository repository = mockRepository.Object;
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            var validator = new UserValidator();

            //Act
            IUserService userService = new UserService(repository, mockMapper.Object, validator);

            //Assert
            Assert.NotNull(userService);
            Assert.True(userService is UserService);
        }

        //Test 1.2
        [Fact]
        public void CreateUserServiceWithInvalidRepository()
        {
            //Arrange
            IUserService userService = null;
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            var validator = new UserValidator();

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => userService = new UserService(null, mockMapper.Object, validator));

            Assert.Equal("Missing repository", ex.Message);
            Assert.Null(userService);
        }

        //Test 1.3 - Valid inputs
        [Theory]
        [InlineData("MartinK", "martink@yahoo.com", "hackme", "Client")]    //Valid client user
        [InlineData("Charlie", "penguinz0@yahoo.com", "hackme", "Coach")]   //Valid coach user
        public void CreateValidUser(string username, string email, string password, string usertype)
        {
            // Arrange
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            IUserRepository repository = userRepositoryMock.Object;

            RegisterUserDTO validUserDTO = new RegisterUserDTO { Username = username, Email = email, Password = password, Usertype = usertype };
            User validUser = new User { Username = username, Email = email, Password = password, Usertype = usertype };

            userRepositoryMock.Setup(x => x.CreateUser(validUser)).Returns(validUser);

            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(validUserDTO)).Returns(validUser);

            var validator = new UserValidator();

            IUserService service = new UserService(repository, mockMapper.Object, validator);

            // Act
            User result = service.CreateUser(validUserDTO);

            // Assert
            Assert.Equal(validUser.Username, result.Username);
            userRepositoryMock.Verify(x => x.CreateUser(validUser), Times.Once);
        }

        //Test 1.4 - Invalid inputs
        [Theory]
        [InlineData("", "penguinz0@yahoo.com", "hackme", "Coach")]              //Username is an empty string
        [InlineData(null, "penguinz0@yahoo.com", "hackme", "Coach")]            //Username is null
        [InlineData("Charlie", "", "hackme", "Coach")]                          //Email is an empty string
        [InlineData("Charlie", null, "hackme", "Coach")]                        //Email is null
        [InlineData("Charlie", "penguinz0@yahoo.com", "", "Coach")]             //Password is an empty string
        [InlineData("Charlie", "penguinz0@yahoo.com", null, "Coach")]           //Password is null
        [InlineData("Charlie", "penguinz0@yahoo.com", "hackme", "")]            //UserType is an empty string
        [InlineData("Charlie", "penguinz0@yahoo.com", "hackme", null)]          //UserType is null
        [InlineData("Charlie", "penguinz0@yahoo.com", "hackme", "UserType")]    //UserType is not "Client" or "Coach"
        public void CreateInvalidUser(string username, string email, string password, string usertype)
        {
            // Arrange
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            IUserRepository repository = userRepositoryMock.Object;

            RegisterUserDTO invalidUserDTO = new RegisterUserDTO { Username = username, Email = email, Password = password, Usertype = usertype };
            User invalidUser = new User { Username = username, Email = email, Password = password, Usertype = usertype };

            userRepositoryMock.Setup(x => x.CreateUser(invalidUser)).Returns(invalidUser);

            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(invalidUserDTO)).Returns(invalidUser);
            var validator = new UserValidator();

            IUserService service = new UserService(repository, mockMapper.Object, validator);

            //Act + Assert
            
            Assert.Throws<ValidationException>(() => service.CreateUser(invalidUserDTO));

            userRepositoryMock.Verify(x => x.CreateUser(invalidUser), Times.Never);
        }

    }
}