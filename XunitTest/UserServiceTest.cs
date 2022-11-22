using Application;
using Application.Interfaces;
using Domain;
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

            //Act
            IUserService userService = new UserService(repository);

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

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => userService = new UserService(null));

            Assert.Equal("Missing repository", ex.Message);
            Assert.Null(userService);
        }

        //Test 1.3 - Valid inputs
        [Theory]
        [InlineData("MartinK", "martink@yahoo.com", "hackme", "Client")] 
        [InlineData("Charlie", "penguinz0@yahoo.com", "hackme", "Coach")] 
        public void CreateValidUser(string username, string email, string password, string usertype)
        {
            // Arrange
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            IUserRepository repository = userRepositoryMock.Object;

            User fakeUser = new User { Username = username, Email = email, Password = password, Usertype = usertype };

            userRepositoryMock.Setup(x => x.CreateUser(username, password, email, usertype)).Returns(fakeUser);

            IUserService service = new UserService(repository);

            // Act
            User result = service.CreateUser(username, password, email, usertype);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeUser, result);
            userRepositoryMock.Verify(x => x.CreateUser(username, password, email, usertype), Times.Once);
        }

        //Test 1.4 - Invalid inputs
        [Theory]
        [InlineData("", "penguinz0@yahoo.com", "hackme", "Coach")]
        [InlineData(null, "penguinz0@yahoo.com", "hackme", "Coach")]
        [InlineData("Charlie", "", "hackme", "Coach")]
        [InlineData("Charlie", null, "hackme", "Coach")]
        [InlineData("Charlie", "penguinz0@yahoo.com", "", "Coach")]
        [InlineData("Charlie", "penguinz0@yahoo.com", null, "Coach")]
        [InlineData("Charlie", "penguinz0@yahoo.com", "hackme", "")]
        [InlineData("Charlie", "penguinz0@yahoo.com", "hackme", null)]
        [InlineData("Charlie", "penguinz0@yahoo.com", "hackme", "UserType")]
        public void CreateInvalidUser(string username, string email, string password, string usertype)
        {
            // Arrange
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            IUserRepository repository = userRepositoryMock.Object;

            User fakeUser = new User { Username = username, Email = email, Password = password, Usertype = usertype };

            userRepositoryMock.Setup(x => x.CreateUser(username, password, email, usertype)).Returns(fakeUser);

            IUserService service = new UserService(repository);

            //Act + Assert
            
            Assert.Throws<ArgumentException>(() => service.CreateUser(username, password, email, usertype));

            userRepositoryMock.Verify(x => x.CreateUser(username, password, email, usertype), Times.Never);
        }

    }
}