using Application;
using Application.DTOs;
using Application.Helpers;
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
        //Test 1.1 - Successfully create UserService with valid repository
        [Fact]
        public void CreateUserServiceWithValidRepository()
        {
            //Arrange
            Mock<IUserRepository> mockRepository = new Mock<IUserRepository>();
            IUserRepository repository = mockRepository.Object;
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            var registrationValidator = new UserRegistrationValidator();

            //Act
            IUserService userService = new UserService(repository, mockMapper.Object, registrationValidator);

            //Assert
            Assert.NotNull(userService);
            Assert.True(userService is UserService);
        }

        //Test 1.2 - Throw ArgumentException when trying to create UserService with invalid repository
        [Fact]
        public void CreateUserServiceWithInvalidRepository()
        {
            //Arrange
            IUserService userService = null;

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => userService = new UserService(null, null, null));

            Assert.Equal("Missing repository", ex.Message);
            Assert.Null(userService);
        }

        //Test 1.3 - Successfully create a user with valid inputs (name, email, password & user type)
        [Theory]
        [InlineData("MartinK", "martink@yahoo.com", "hackme", "Client")]    //Valid client user
        [InlineData("Charlie", "penguinz0@yahoo.com", "hackme", "Coach")]   //Valid coach user
        public void CreateValidUser(string username, string email, string password, string usertype)
        {
            // Arrange
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            IUserRepository repository = userRepositoryMock.Object;

            RegisterUserDTO validUserDTO = new RegisterUserDTO { Username = username, Email = email, Password = password, Usertype = usertype };
            User validUser = null;

            switch (usertype)
            {
                case "Client":
                    validUser = new Client { Username = username, Email = email, Password = password, Usertype = usertype };
                    break;

                case "Coach":
                    validUser = new Coach { Username = username, Email = email, Password = password, Usertype = usertype };
                    break;
            }

            userRepositoryMock.Setup(x => x.CreateUser(validUser)).Returns(validUser);

            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(validUserDTO)).Returns(validUser);

            var registrationValidator = new UserRegistrationValidator();

            IUserService service = new UserService(repository, mockMapper.Object, registrationValidator);

            // Act
            User result = service.CreateUser(validUserDTO);

            // Assert
            Assert.Equal(validUser.Username, result.Username);
            userRepositoryMock.Verify(x => x.CreateUser(validUser), Times.Once);
        }

        //Test 1.4 - Throw ArgumentException when trying to create user with invalid inputs
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
            var registrationValidator = new UserRegistrationValidator();

            IUserService service = new UserService(repository, mockMapper.Object, registrationValidator);

            //Act + Assert

            Assert.Throws<ValidationException>(() => service.CreateUser(invalidUserDTO));

            userRepositoryMock.Verify(x => x.CreateUser(invalidUser), Times.Never);
        }

        //Test 2.1 - Valid login credentials
        [Theory]
        [InlineData("Charlie", "hackme")]    //Valid user
        public void GetValidUserByUsername(string username, string password)
        {
            // Arrange
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            IUserRepository repository = userRepositoryMock.Object;

            User validUser = new User { Username = username, Password = PasswordHelper.HashPasswordBCrypt(password) };
            LoginUserDTO validUserDTO = new LoginUserDTO { Username = username, Password = password };

            userRepositoryMock.Setup(x => x.ReadUserByUsername(username)).Returns(validUser);

            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(validUserDTO)).Returns(validUser);

            var registrationValidator = new UserRegistrationValidator();

            IUserService service = new UserService(repository, mockMapper.Object, registrationValidator);

            // Act
            User result = service.GetUserByUsername(validUserDTO);

            // Assert
            Assert.Equal(validUser.Username, result.Username);
            Assert.Equal(validUser.Password, result.Password);
            userRepositoryMock.Verify(x => x.ReadUserByUsername(username), Times.Once);
        }

        //Test 2.2 - Invalid login credentials
        [Theory]
        [InlineData("Charlie", "hackme")]    //Valid user
        public void GetInvalidUserByUsername(string username, string password)
        {
            // Arrange
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            IUserRepository repository = userRepositoryMock.Object;

            User validUser = new User { Username = username, Password = PasswordHelper.HashPasswordBCrypt("test") };
            LoginUserDTO validUserDTO = new LoginUserDTO { Username = username, Password = password };

            userRepositoryMock.Setup(x => x.ReadUserByUsername(username)).Returns(validUser);

            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(validUserDTO)).Returns(validUser);

            var registrationValidator = new UserRegistrationValidator();

            IUserService service = new UserService(repository, mockMapper.Object, registrationValidator);

            // Act + assert
            Assert.Throws<ValidationException>(() => service.GetUserByUsername(validUserDTO)).Message.Equals("Wrong login credentials");
            userRepositoryMock.Verify(x => x.ReadUserByUsername(username), Times.Once);
        }

        //Test 4.1 - Valid update user inputs
        [Theory]
        [InlineData(1, "Charlie", "penguinz0@yahoo.com", "hackme", "hackme")]    //Valid user
        [InlineData(2, "Charlie", "penguinz0@yahoo.com", "hackme1", "hackme1")]    //Valid user
        public void UpdateValidUser(int id, string username, string email, string password, string oldPassword)
        {
            // Arrange
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            IUserRepository repository = userRepositoryMock.Object;

            User validUser = new User { Id = id, Username = username, Password = PasswordHelper.HashPasswordBCrypt(password), Email = email };
            UpdateUserDTO validUserDTO = new UpdateUserDTO { /*Username = username,*/ Password = password, /*Email = email*/ OldPassword = oldPassword };

            userRepositoryMock.Setup(x => x.UpdateUser(id, validUser, oldPassword)).Returns(validUser);

            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(validUserDTO)).Returns(validUser);

            var registrationValidator = new UserRegistrationValidator();

            IUserService service = new UserService(repository, mockMapper.Object, registrationValidator);

            // Act
            User result = service.UpdateUser(id, validUserDTO);

            // Assert
            Assert.NotNull(validUser);
            Assert.Equal(validUser, result);
            userRepositoryMock.Verify(x => x.UpdateUser(id, validUser, oldPassword), Times.Once);
        }

        //Test 4.2 - Invalid update user inputs
        [Theory]
        [InlineData(-1, "Charlie", "penguinz0@yahoo.com", "hackme", "hacke")]    // Invalid user
        public void UpdateInvalidUser(int id, string username, string email, string password, string oldPassword)
        {
            // Arrange
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            IUserRepository repository = userRepositoryMock.Object;

            User invalidUser = new User { Id = id, Username = username, Password = PasswordHelper.HashPasswordBCrypt(password), Email = email };
            UpdateUserDTO validUserDTO = new UpdateUserDTO { /*Username = username,*/ Password = password, /*Email = email*/ OldPassword = oldPassword };

            userRepositoryMock.Setup(x => x.UpdateUser(id, invalidUser, oldPassword)).Returns(invalidUser);

            Mock<IMapper> mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(validUserDTO)).Returns(invalidUser);

            var registrationValidator = new UserRegistrationValidator();

            IUserService service = new UserService(repository, mockMapper.Object, registrationValidator);

            // Act + assert
            Assert.Throws<ArgumentException>(() => service.UpdateUser(id, validUserDTO));
            userRepositoryMock.Verify(x => x.UpdateUser(id, invalidUser, oldPassword), Times.Never);
        }

        [Fact]
        public void GetAllUsers()
        {
            // Arrange
            Mock<IUserRepository> mockRepo = new Mock<IUserRepository>();
            IUserRepository repo = mockRepo.Object;

            var fakeRepo = new User[]
            {
                new User { Id = 1, Email = "email@email.com", Username = "test", Password = "test", Usertype = "Coach"},
            };

            mockRepo.Setup(r => r.ReadAllUsers()).Returns(fakeRepo.ToList());

            IUserService service = new UserService(repo, null, null);

            // Act
            var result = service.GetAllUsers();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            mockRepo.Verify(r => r.ReadAllUsers(), Times.Once);
        }


        [Theory]
        [InlineData(1, "", "penguinz0@yahoo.com", "hackme", "Coach")] // Id is valid and not 0
        [InlineData(2, "", "test@@yahoo.com", "hackme", "Client")] // Id is valid and not 0
        public void DeleteValidUser(int id, string username, string email, string password, string usertype)
        {
            // Arrange
            Mock<IUserRepository> mockRepo = new Mock<IUserRepository>();
            IUserRepository repository = mockRepo.Object;

            IUserService service = new UserService(repository, null, null);
            User fakeUser = new User { Id = id, Username = username, Email = email, Password = password, Usertype = usertype };

            mockRepo.Setup(r => r.DeleteUser(id)).Returns(fakeUser);

            // Act
            User result = service.DeleteUser(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeUser, result);
            mockRepo.Verify(r => r.DeleteUser(id), Times.Once);
        }

        [Theory]
        [InlineData(-1, "", "test0@yahoo.com", "hackme", "Client")] // Id is invalid and not above 0
        [InlineData(0, "", "penguinz0@yahoo.com", "hackme", "Coach")] // Id is invalid and not above 0
        public void DeleteInvalidUser(int id, string username, string email, string password, string usertype)
        {
            // Arrange
            Mock<IUserRepository> mockRepo = new Mock<IUserRepository>();
            IUserRepository repository = mockRepo.Object;

            IUserService service = new UserService(repository, null, null);
            User fakeUser = new User { Id = id, Username = username, Email = email, Password = password, Usertype = usertype };

            mockRepo.Setup(r => r.DeleteUser(id)).Returns(fakeUser);

            // Act + assert
            Assert.Throws<ArgumentException>(() => service.DeleteUser(id));
            mockRepo.Verify(r => r.DeleteUser(id), Times.Never);
        }

        [Theory]
        [InlineData(1, "penguinz0", "penguinz0@yahoo.com", "hackme", "Coach")] // Id is valid and not 0
        [InlineData(2, "test", "test@@yahoo.com", "hackme", "Client")] // Id is valid and not 0
        public void GetValidUser(int id, string username, string email, string password, string usertype)
        {
            // Arrange
            Mock<IUserRepository> mockRepo = new Mock<IUserRepository>();
            IUserRepository repository = mockRepo.Object;

            IUserService service = new UserService(repository, null, null);
            User fakeUser = new User { Id = id, Username = username, Email = email, Password = password, Usertype = usertype };

            mockRepo.Setup(r => r.ReadUserById(id)).Returns(fakeUser);

            // Act
            User result = service.GetUser(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeUser, result);
            mockRepo.Verify(r => r.ReadUserById(id), Times.Once);
        }
    }
}