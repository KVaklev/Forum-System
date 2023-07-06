using Business.Exceptions;
using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using ForumManagementSystem.Services;
using ForumManagementSystem.Tests.Helpers;
using Moq;

namespace ForumManagementSystem.Tests.ServicesTests
{
    [TestClass]
    public class UserServicesTests
    {
        [TestMethod]
        public void GetById_Should_ReturnCorrectUser_When_ParametersAreValid()
        {
            //Arrange

            User expectedUser = TestHelpers.GetTestUser();
          
            var userRepositoryMock= new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetById(1))
                .Returns(expectedUser);
            
            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act

            User actualUser = sut.GetById(expectedUser.Id);

            //Assert

            Assert.AreEqual(expectedUser, actualUser);

        }

        [TestMethod]
        public void GetById_Should_ThrowException_When_UserIsNotFound()
        {
            //Arrange

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Throws(new EntityNotFoundException("User doesn't exist."));

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);
            
            //Act & Assert
            Assert.ThrowsException<EntityNotFoundException>(() => sut.GetById(1));

        }

        [TestMethod]
        public void CreateUser_Should_ReturnCorrectUser_When_ParametersAreValid()
        {
            //Arrange

            User createdUser = TestHelpers.GetTestCreateUser();
           
            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.Create(createdUser))
                .Returns(createdUser);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act

            User actualUser = sut.Create(createdUser);

            //Assert

            Assert.AreEqual(createdUser, actualUser);
        }

        [TestMethod]
        public void CreateUser_Should_ThrowException_When_ParametersAreNotValid()
        {
            //Arrange

            User createdUser = TestHelpers.GetTestCreateUser();
            
            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.UsernameExists(It.IsAny<string>()))
                .Returns(true);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<DuplicateEntityException>(()=>sut.Create(createdUser));
        }

        [TestMethod]
        public void CreateUser_Should_ThrowException_When_AnotherParametersAreNotValid()
        {
            //Arrange

            User createdUser = TestHelpers.GetTestCreateUser();

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.EmailExists(It.IsAny<string>()))
                .Returns(true);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<DuplicateEntityException>(() => sut.Create(createdUser));
        }

        [TestMethod]
        public void DeleteUser_Should_ThrowException_When_UserIsNotCreator()
        {
            //Arrange 

            User loggedUser = TestHelpers.GetTestUser();
            
            User userToDelete = TestHelpers.GetTestDeleteUser();
     
            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetById(userToDelete.Id))
                .Returns(userToDelete);
            likeCommentRepositoryMock
                .Setup(repo=>repo.DeletedByUser(userToDelete))
                .Returns(true);
            userRepositoryMock
                .Setup(repo=>repo.Delete(userToDelete.Id))
                .Returns(userToDelete);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException <UnauthorizedOperationException>(() => sut.Delete(userToDelete.Id, loggedUser));

        }

        [TestMethod]
        public void DeleteUser_Should__When_UserIsAdmin()
        {
            //Arrange 

            User loggedUser = TestHelpers.GetTestUserAdmin();
            
            User userToDelete = TestHelpers.GetTestDeleteUser();
           
            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetById(userToDelete.Id))
                .Returns(userToDelete);
            likeCommentRepositoryMock
                .Setup(repo => repo.DeletedByUser(userToDelete))
                .Returns(true);
            userRepositoryMock
                .Setup(repo => repo.Delete(userToDelete.Id))
                .Returns(userToDelete);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act 

            sut.Delete(userToDelete.Id, loggedUser);

            //Assert

            userRepositoryMock
                .Verify(repo => repo.Delete(userToDelete.Id), Times.Once);
            likeCommentRepositoryMock
                .Verify(repo => repo.DeletedByUser(userToDelete), Times.Once);
        }

        [TestMethod]
        public void GetAll_Should_ReturnCorrectList_When_ParametersAreValid()
        {
            //Arrange

            List<User> users = TestHelpers.GetTestListUsers();
           
            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetAll())
                .Returns(users);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act

            var expectedUsers = sut.GetAll();

            //Assert

            Assert.AreEqual(expectedUsers, users);
        }

        [TestMethod]
        public void GetByUsername_Should_ReturnCorrectUser_When_ParametersAreValid()
        {
            //Arrange

            User expectedUser = TestHelpers.GetTestUser();

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetByUsername(expectedUser.Username))
                .Returns(expectedUser);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act

            User actualUser = sut.GetByUsername(expectedUser.Username);

            //Assert

            Assert.AreEqual(expectedUser, actualUser);
        }

        [TestMethod]
        public void UpdateUser_Should_ReturnCorrectUser_When_ParametersAreValid()
        {
            //Arrange

            User userToUpdate = TestHelpers.GetTestUpdateUser();
            
            User loggedUser = TestHelpers.GetTestUpdateUser();
       
            User user = TestHelpers.GetTestUpdateUserInfo();
           
            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo=>repo.GetById(2))
                .Returns(userToUpdate);
            userRepositoryMock
                .Setup(repo => repo.Update(userToUpdate.Id, user, loggedUser))
                .Returns(user);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act

            User updatedUser = sut.Update(userToUpdate.Id, user, loggedUser);

            //Assert

            Assert.AreEqual(user, updatedUser);
        }

        [TestMethod]
        public void UpdateUser_Should_ThrowException_When_TriesToChangeUsername()
        {
            //Arrange

            User userToUpdate = TestHelpers.GetTestUpdateUser();

            User loggedUser = TestHelpers.GetTestUpdateUser();

            User user = new User()
            {
                Id = 2,
                FirstName = "Mareto",
                LastName = "Petrovka",
                Username = "Mandarinka"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetById(userToUpdate.Id))
                .Returns(userToUpdate);
            userRepositoryMock
                .Setup(repo => repo.Update(userToUpdate.Id, user, loggedUser))
                .Returns(user);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<InvalidOperationException>(() => sut.Update(userToUpdate.Id, user, loggedUser));
        }

        [TestMethod]
        public void UpdateUser_Should_ThrowException_When_UpdatorIsNotAuthorized()
        {
            //Arrange

            User userToUpdate = TestHelpers.GetTestUpdateUser();

            User loggedUser = TestHelpers.GetTestUser();
         
            User user = new User()
            {
                Id = 2,
                FirstName = "Mareto",
                LastName = "Petrovka",
                Username = "Mandarinka"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetById(userToUpdate.Id))
                .Returns(userToUpdate);
            userRepositoryMock
                .Setup(repo => repo.Update(userToUpdate.Id, user, loggedUser))
                .Returns(user);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<UnauthorizedOperationException>(() => sut.Update(userToUpdate.Id, user, loggedUser));
        }

        [TestMethod]
        public void UpdateUser_Should_ThrowException_When_EmailExists()
        {
            //Arrange

            User userToUpdate = TestHelpers.GetTestUpdateUser();

            User loggedUser = TestHelpers.GetTestUpdateUser();

            User user = new User()
            {
                Id = 2,
                FirstName = "Mareto",
                LastName = "Petrovka",
                Email = "m.dobreva@gmail.com"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetById(userToUpdate.Id))
                .Returns(userToUpdate);
            userRepositoryMock
                .Setup(repo => repo.EmailExists(user.Email))
                .Returns(true);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<DuplicateEntityException>(() => sut.Update(userToUpdate.Id, user, loggedUser));
        }

        [TestMethod]
        public void Promote_Should_ReturnUserPromoted_When_ParametersAreValid()
        {
            //Arrange

            User userToPromote = TestHelpers.GetTestUpdateUser();

            User expectedUser = TestHelpers.GetTestExpectedUserAsAdmin();

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.Promote(userToPromote))
                .Returns(expectedUser);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act

            User actualUser = sut.Promote(userToPromote);

            //Assert

            Assert.AreEqual(expectedUser, actualUser);

        }

        [TestMethod]
        public void Block_Should_ReturnUserBlocked_When_ParametersAreValid()
        {
            //Arrange

            User userToBlock = TestHelpers.GetTestUpdateUser();

            User expectedUser = TestHelpers.GetTestExpectedUserAsBlocked();

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.BlockUser(userToBlock))
                .Returns(expectedUser);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act

            User actualUser = sut.BlockUser(userToBlock);

            //Assert

            Assert.AreEqual(expectedUser, actualUser);

        }

        [TestMethod]
        public void Unblock_Should_ReturnUserUnblocked_When_ParametersAreValid()
        {
            //Arrange

            User userToUnblock = TestHelpers.GetTestExpectedUserAsBlocked();

            User expectedUser = TestHelpers.GetTestExpectedUserAsUnblocked();

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.UnblockUser(userToUnblock))
                .Returns(expectedUser);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act

            User actualUser = sut.UnblockUser(userToUnblock);

            //Assert

            Assert.AreEqual(expectedUser, actualUser);
        }

        //[TestMethod]
        //public void FilterBy_Should_ReturnCorrectList_When_ParametersAreValid()
        //{
        //    //Arrange

        //    List<User> users = TestHelpers.GetTestListUsers();

        //    List<User> expectedUsers = TestHelpers.GetTestExpectedListUsers();

        //    UserQueryParameters filterParameters = new UserQueryParameters()
        //    {
        //        FirstName = "Ivan"
        //    };

        //    var userRepositoryMock = new Mock<IUserRepository>();
        //    var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

        //    userRepositoryMock
        //        .Setup(repo => repo.FilterBy(filterParameters))
        //        .Returns(expectedUsers);

        //    var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

        //    // Act

        //    List<User> filteredUsers = sut.FilterBy(filterParameters);

        //    // Assert

        //    Assert.AreEqual(filteredUsers, expectedUsers);
        //}
    }
}