using AutoMapper;
using Business.Exceptions;
using ForumManagementSystem.Controllers;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using ForumManagementSystem.Tests.Helpers;
using Moq;
using Presentation.Helpers;


namespace ForumManagementSystem.Tests.ControllersTests
{
    [TestClass]
    public class UsersApiControllersTests
    {
        [TestMethod]
        public void GetUserById_Should_When_ParametersAreValid()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";

            User loggedUser = TestHelpers.GetTestUserAdmin();
            User userToTest = TestHelpers.GetTestUser();

            GetUserDto expectedUserDto = new GetUserDto()
            {
                FirstName = userToTest.FirstName,
                LastName = userToTest.LastName,
                Email = userToTest.Email,
                IsAdmin = userToTest.IsAdmin,
                Username = userToTest.Username
            };

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);
            userServiceMock
                .Setup(us => us.GetById(userToTest.Id))
                .Returns(userToTest);
            mapperMock
                .Setup(m => m.Map<GetUserDto>(It.IsAny<User>()))
                .Returns(expectedUserDto);

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.GetUserById(credentials, userToTest.Id);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUserById_Should_ThrowException_WhenUserIsNotAuthorized()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:23";

            User loggedUser = TestHelpers.GetTestUser();
            User userToTest = TestHelpers.GetTestUser();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Throws(new UnauthorizedOperationException(""));

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.GetUserById(credentials, userToTest.Id);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUserById_Should_ThrowException_WhenUserIsNotFound()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:23";

            User loggedUser = TestHelpers.GetTestUser();
            User userToTest = TestHelpers.GetTestUser();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Throws(new EntityNotFoundException(""));

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.GetUserById(credentials, userToTest.Id);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUserById_Should_ReturnUnauthorized_When_UserIsNotAuthorized()
        {
            //Arrange
            string credentials = "ivanchoDraganchov:23";

            User loggedUser = null;
            User userToTest = TestHelpers.GetTestUser();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.GetUserById(credentials, userToTest.Id);

            //Assert

            Assert.IsNotNull(result);
        }
    }
}
