using Application.Interfaces;
using Application;
using Moq;
using Application.DTOs;
using Application.Validators;
using AutoMapper;
using Domain;
using Application.Helpers;

namespace XunitTest
{
    public class AuthenticationServiceTest
    {
        JwtConfig _jwtConfig = new JwtConfig()
        {
            Secret = "Team AC Game Coach Service"
        };

        User[] fakeRepo = new User[]
        {
                new User { Username = "MartinK", Email = "martink@yahoo.com", Password = "hackme", Usertype = "Client" },
                new User { Username = "Charlie", Email = "penguinz0@yahoo.com", Password = "hackme", Usertype = "Coach" }
        };

        //Test 1.1  
        [Fact]
        public void CreateAuthenticationServiceWithValidRepository()
        {
            //Arrange
            Mock<IUserRepository> mockRepository = new Mock<IUserRepository>();
            IUserRepository repository = mockRepository.Object;

            //Act
            IAuthenticationService authenticationService = new AuthenticationService(repository, null, _jwtConfig);

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
            var ex = Assert.Throws<ArgumentException>(() => authenticationService = new AuthenticationService(null, null, _jwtConfig));

            Assert.Equal("Missing repository", ex.Message);
            Assert.Null(authenticationService);
        }

        [Theory]
        [InlineData("MartinK", "hackme")]    //Valid user 1
        [InlineData("Charlie", "hackme")]   //Valid user 2
        public void LoginWithValidCredentials(string username, string password)
        {
            // Arrange
            Mock<IUserRepository> mockRepository = new Mock<IUserRepository>();
            IUserRepository repository = mockRepository.Object;

            LoginUserDTO validUserDTO = new LoginUserDTO { Username = username, Password = password };
            User validUser = fakeRepo.FirstOrDefault(x => x.Username == username);

            mockRepository.Setup(r => r.ReadUserByUsername(username)).Returns(validUser);


            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(validUserDTO)).Returns(validUser);

            IAuthenticationService authenticationService = new AuthenticationService(repository, mockMapper.Object, _jwtConfig);

            // Act
            var expected = authenticationService.Login(validUserDTO);

            // Assert
            Assert.NotNull(expected);
            Assert.NotNull(expected.Token);
            mockRepository.Verify(r => r.ReadUserByUsername(username), Times.Once);
        }

        [Theory]
        [InlineData("MartinK1", "hackme")]    //Invalid user 1
        [InlineData("Charlie1", "hackme")]   //Invalid user 2
        public void LoginWithInvalidCredentials(string username, string password)
        {
            // Arrange
            Mock<IUserRepository> mockRepository = new Mock<IUserRepository>();
            IUserRepository repository = mockRepository.Object;

            LoginUserDTO invalidUserDTO = new LoginUserDTO { Username = username, Password = password };
            User invalidUser = fakeRepo.FirstOrDefault(x => x.Username == username);

            mockRepository.Setup(r => r.ReadUserByUsername(username)).Returns(invalidUser);

            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(invalidUserDTO)).Returns(invalidUser);
            IAuthenticationService authenticationService = new AuthenticationService(repository, mockMapper.Object, _jwtConfig);

            // Act + assert
            Assert.Throws<KeyNotFoundException>(() => authenticationService.Login(invalidUserDTO));
            mockRepository.Verify(r => r.ReadUserByUsername(username), Times.Once);
        }
    }
}
