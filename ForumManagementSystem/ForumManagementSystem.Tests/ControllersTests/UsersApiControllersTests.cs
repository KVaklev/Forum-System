using AutoMapper;
using Business.Dto;
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

        [TestMethod]
        public void GetUsers_Should_When_ParametersAreValid()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";

            User loggedUser = TestHelpers.GetTestUserAdmin();
            List<User> users = TestHelpers.GetTestListUsers();
            List<GetUserDto> expectedUsers = TestHelpers.GetTestExpectedListDtoUsers();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
               .Setup(am => am.TryGetUser(credentials))
               .Returns(loggedUser);
            userServiceMock
                .Setup(us=>us.FilterBy(It.IsAny<UserQueryParameters>()))
                .Returns(users);
            mapperMock.Setup(m => m.Map<GetUserDto>(It.IsAny<User>()))
                .Returns((User user) =>
            {
                return expectedUsers.FirstOrDefault(dto => dto.FirstName == user.FirstName);
            });

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.GetUsers(credentials, new UserQueryParameters());

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUsers_Should_ReturnUnauthorized_When_UserIsNotAuthorized()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";

            User loggedUser = null;

            List<User> users = TestHelpers.GetTestListUsers();

            List<GetUserDto> expectedUsers = TestHelpers.GetTestExpectedListDtoUsers();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
               .Setup(am => am.TryGetUser(credentials))
               .Returns(loggedUser);
            
            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.GetUsers(credentials, new UserQueryParameters());

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateUser_Should_When_ParametersAreValid()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";

            User user=TestHelpers.GetTestUser();
            CreateUserDto createUserDto = TestHelpers.GetTestCreateUserDto();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(am => am.Map<CreateUserDto>(createUserDto))
                .Returns(createUserDto);
            userServiceMock
                .Setup(us => us.Create(user))
                .Returns(user);
                
            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.CreateUser(createUserDto);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateUser_Should_ReturnConflict_When_DuplicateEntityExists()
        {
            // Arrange

            CreateUserDto createUserDto = TestHelpers.GetTestCreateUserDto();

            DuplicateEntityException exception = new DuplicateEntityException("User already exists.");

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(m => m.Map<User>(createUserDto))
                .Returns(TestHelpers.GetTestUser());
            userServiceMock
                .Setup(us => us.Create(It.IsAny<User>()))
                .Throws(exception);

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            // Act

            var result = sut.CreateUser(createUserDto);

            // Assert

           Assert.IsNotNull (result);
        }
        
        [TestMethod]
        public void DeleteUser_Should_When_ParametersAreValid()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToDelete=TestHelpers.GetTestUser();
            User loggedUser=TestHelpers.GetTestUserAdmin();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am=>am.TryGetUser(credentials))
                .Returns(userToDelete);
            userServiceMock
                .Setup(us => us.Delete(userToDelete.Id, loggedUser))
                .Equals(true);

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.DeleteUser(userToDelete.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteUser_Should_ReturnUnauthorized_When_UserIsNotAuthorized()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToDelete = TestHelpers.GetTestUser();
            User loggedUser = TestHelpers.GetTestUserAdmin();
            
            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);
            userServiceMock
                .Setup(us => us.Delete(userToDelete.Id, loggedUser))
                .Throws(new UnauthorizedOperationException("Unauthorized to delete user."));

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.DeleteUser(userToDelete.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteUser_Should_ReturnNotFound_WhenIDIsNotFound()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToDelete = TestHelpers.GetTestUser();
            User loggedUser = TestHelpers.GetTestUserAdmin();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);
            userServiceMock
                .Setup(us => us.Delete(userToDelete.Id, loggedUser))
                .Throws(new EntityNotFoundException("User not found."));

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.DeleteUser(userToDelete.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Promote_Should_When_ParametersAreValid()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToPromote = TestHelpers.GetTestUser();
            User loggedUser = TestHelpers.GetTestUserAdmin();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);
            userServiceMock
                .Setup(us=>us.GetById(userToPromote.Id))
                .Returns(userToPromote);
            userServiceMock
                .Setup(us=>us.Promote(userToPromote))
                .Returns(userToPromote);

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.Promote(userToPromote.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Promote_Should_ReturnNotAllowed_When_UserIsNotAdmin()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToPromote = TestHelpers.GetTestUser();
            User loggedUser = TestHelpers.GetTestUser();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);
           
            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.Promote(userToPromote.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Promote_Should_ReturnUnauthorized_When_UserIsNotAuthorized()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToPromote = TestHelpers.GetTestUser();
            User loggedUser = TestHelpers.GetTestUser();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Throws(new UnauthorizedOperationException("Unauthorized to promote user."));
         
            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.Promote(userToPromote.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Promote_Should_ReturnNotFound_WhenIDIsNotFound()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToPromote = TestHelpers.GetTestUser();
            User loggedUser = TestHelpers.GetTestUserAdmin();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);
            userServiceMock
                .Setup(us => us.GetById(userToPromote.Id))
                .Throws(new EntityNotFoundException("User not found."));
        
            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.Promote(userToPromote.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Block_Should_When_ParametersAreValid()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToBlock = TestHelpers.GetTestUser();
            User loggedUser = TestHelpers.GetTestUserAdmin();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);
            userServiceMock
                .Setup(us => us.GetById(userToBlock.Id))
                .Returns(userToBlock);
            userServiceMock
                .Setup(us => us.BlockUser(userToBlock))
                .Returns(userToBlock);

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.BlockUser(userToBlock.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Block_Should_ReturnNotAllowed_When_UserIsNotAdmin()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToBlock = TestHelpers.GetTestUser();
            User loggedUser = TestHelpers.GetTestUser();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.BlockUser(userToBlock.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Block_Should_ReturnUnauthorized_When_UserIsNotAuthorized()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToBlock = TestHelpers.GetTestUser();
            User loggedUser = TestHelpers.GetTestUser();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Throws(new UnauthorizedOperationException("Unauthorized to block user."));

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.BlockUser(userToBlock.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Block_Should_ReturnNotFound_WhenIDIsNotFound()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToBlock = TestHelpers.GetTestUser();
            User loggedUser = TestHelpers.GetTestUserAdmin();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);
            userServiceMock
                .Setup(us => us.GetById(userToBlock.Id))
                .Throws(new EntityNotFoundException("User not found."));

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.BlockUser(userToBlock.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Unblock_Should_When_ParametersAreValid()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToUnblock = TestHelpers.GetTestExpectedUserAsBlocked();
            User loggedUser = TestHelpers.GetTestUserAdmin();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);
            userServiceMock
                .Setup(us => us.GetById(userToUnblock.Id))
                .Returns(userToUnblock);
            userServiceMock
                .Setup(us => us.UnblockUser(userToUnblock))
                .Returns(userToUnblock);

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.UnblockUser(userToUnblock.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Unblock_Should_ReturnNotAllowed_When_UserIsNotAdmin()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToUnblock = TestHelpers.GetTestExpectedUserAsBlocked();
            User loggedUser = TestHelpers.GetTestUser();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.UnblockUser(userToUnblock.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Unblock_Should_ReturnUnauthorized_When_UserIsNotAuthorized()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToUnblock = TestHelpers.GetTestExpectedUserAsBlocked();
            User loggedUser = TestHelpers.GetTestUser();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Throws(new UnauthorizedOperationException("Unauthorized to block user."));

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.UnblockUser(userToUnblock.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Unblock_Should_ReturnNotFound_WhenIDIsNotFound()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";
            User userToUnblock = TestHelpers.GetTestUser();
            User loggedUser = TestHelpers.GetTestUserAdmin();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Returns(loggedUser);
            userServiceMock
                .Setup(us => us.GetById(userToUnblock.Id))
                .Throws(new EntityNotFoundException("User not found."));

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.UnblockUser(userToUnblock.Id, credentials);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateUser_Should_When_ParametersAreValid()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";

            User loggedUser = TestHelpers.GetTestUserAdmin();
            User userToUpdate = TestHelpers.GetTestUser();
            UpdateUserDto updateUserDto = TestHelpers.GetTestUpdateUserDto();
            
            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(am => am.Map<UpdateUserDto>(updateUserDto))
                .Returns(updateUserDto);
            userServiceMock
                .Setup(us => us.Update(userToUpdate.Id, userToUpdate, loggedUser))
                .Returns(userToUpdate);

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.UpdateUser(userToUpdate.Id, credentials, updateUserDto);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateUser_Should_Throw_When_UserIsNotAuthorized()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";

            User loggedUser = TestHelpers.GetTestUserAdmin();
            User userToUpdate = TestHelpers.GetTestUser();
            UpdateUserDto updateUserDto = TestHelpers.GetTestUpdateUserDto();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                .Setup(am => am.TryGetUser(credentials))
                .Throws(new UnauthorizedOperationException("Unauthorized to update"));

            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.UpdateUser(userToUpdate.Id, credentials, updateUserDto);

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateUser_Should_Throw_When_UserToLogInIsNotFound()
        {
            //Arrange

            string credentials = "ivanchoDraganchov:123";

            User userToUpdate = TestHelpers.GetTestUser();
            UpdateUserDto updateUserDto = TestHelpers.GetTestUpdateUserDto();

            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();
            var mapperMock = new Mock<IMapper>();

            authManagerMock
                 .Setup(am => am.TryGetUser(credentials))
                 .Throws(new EntityNotFoundException("User not found."));
           
            var sut = new UsersApiController(userServiceMock.Object, mapperMock.Object, authManagerMock.Object);

            //Act

            var result = sut.UpdateUser(userToUpdate.Id, credentials, updateUserDto);

            //Assert

            Assert.IsNotNull(result);
        }

    }
}
