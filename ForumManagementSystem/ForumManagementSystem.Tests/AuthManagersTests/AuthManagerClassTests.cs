using Business.Exceptions;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Presentation.Helpers;

namespace ForumManagementSystem.Tests.AuthManagersTests
{
    [TestClass]
    public class AuthManagerClassTests
    {
        [TestMethod]
        public void TryGetUser_Should_ReturnCorrectUser_When_ParametersAreValid()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";

            User expectedUser = new User()
            {
                Id = 1,
                FirstName = "Ivan",
                LastName = "Draganov",
                Email = "i.draganov@gmail.com",
                Username = "ivanchoDraganchov",
                Password = "MTIz",
                PhoneNumber = "0897556285",
                IsAdmin = true,
                IsBlocked = false
            };

            var userServiceMock = new Mock<IUserService>();
            var contextAccessor = new Mock<IHttpContextAccessor>();

            userServiceMock
                .Setup(userService=>userService.GetByUsername("ivanchoDraganchov"))
                .Returns(expectedUser);

            var sut = new AuthManager(userServiceMock.Object, contextAccessor.Object);

            //Act 

            User actualUser = sut.TryGetUser(credentials);

            //Assert

            Assert.AreEqual(expectedUser, actualUser);

        }

        [TestMethod]
        public void TryGetUser_Should_ThrowException_WhenUsernameIsInvalid()
        {
            //Arrange

            string credentials = "TestUser:13";
            var userServiceMock = new Mock<IUserService>();
			var contextAccessor = new Mock<IHttpContextAccessor>();

			userServiceMock
                .Setup(userService => userService.GetByUsername("TestUser"))
                .Throws(new EntityNotFoundException("Invalid username."));

            var sut = new AuthManager(userServiceMock.Object, contextAccessor.Object);

            //Act & Assert

            Assert.ThrowsException<UnauthorizedOperationException>(() => sut.TryGetUser(credentials));
        }

        [TestMethod]
        public void TryGetUser_Should_ThrowException_WhenCredentialsAreInvalid()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:13";

            User expectedUser = new User()
            {
                Id = 1,
                FirstName = "Ivan",
                LastName = "Draganov",
                Email = "i.draganov@gmail.com",
                Username = "ivanchoDraganchov",
                Password = "MTIz",
                PhoneNumber = "0897556285",
                IsAdmin = true,
                IsBlocked = false
            };
            var userServiceMock = new Mock<IUserService>();
			var contextAccessor = new Mock<IHttpContextAccessor>();

			userServiceMock
                .Setup(userService => userService.GetByUsername("ivanchoDraganchov"))
                .Returns(expectedUser);

            var sut = new AuthManager(userServiceMock.Object, contextAccessor.Object);

            //Act & Assert

            Assert.ThrowsException<UnauthenticatedOperationException>(()=>sut.TryGetUser(credentials));
        }
    }
}
