using Application.Interfaces;
using Application;
using Moq;
using Application.DTOs;
using Application.Validators;
using AutoMapper;
using Domain;

namespace XunitTest
{
    public class AuthenticationServiceTest
    {
        //Test 1.1  
        [Fact]
        public void CreateAuthenticationServiceWithValidRepository()
        {
            //Arrange
            Mock<IUserRepository> mockRepository = new Mock<IUserRepository>();
            IUserRepository repository = mockRepository.Object;

            //Act
            IAuthenticationService authenticationService = new AuthenticationService(repository);

            //Assert
            Assert.NotNull(authenticationService);
            Assert.True(authenticationService is AuthenticationService);
        }

        //Test 1.2
        [Fact]
        public void CreateUserServiceWithInvalidRepository()
        {
            //Arrange
            IAuthenticationService authenticationService = null;


            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => authenticationService = new AuthenticationService(null));

            Assert.Equal("Missing repository", ex.Message);
            Assert.Null(authenticationService);
        }

        [Theory]
        [InlineData("MartinK", "martink@yahoo.com", "hackme", "Client")]    //Valid client user
        [InlineData("Charlie", "penguinz0@yahoo.com", "hackme", "Coach")]   //Valid coach user
        public void LoginWithValidCredentials(string username, string email, string password, string usertype)
        {
            // Arrange
            Mock<IUserRepository> mockRepository = new Mock<IUserRepository>();
            IUserRepository repository = mockRepository.Object;

            LoginUserDTO validUserDTO = new LoginUserDTO { Username = username, Password = password };
            User validUser = new User { Username = username, Email = email, Password = password, Usertype = usertype };

            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(validUserDTO)).Returns(validUser);

            var registrationValidator = new UserRegistrationValidator();
            var loginValidator = new UserLoginValidator();

            IUserService service = new UserService(repository, mockMapper.Object, registrationValidator, loginValidator);
            IAuthenticationService authenticationService = new AuthenticationService(repository);

            // Act


            // Assert
        }
    }
}
