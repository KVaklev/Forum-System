using Business.Exceptions;
using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using ForumManagementSystem.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            User testUser = new User()
            {
                Id = 1,
                FirstName = "TestFirst",
                LastName = "TestLast",
                Email = "test@gmail.com",
                Username = "testUsername",
                Password = "MTIz"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.Create(testUser))
                .Returns(testUser);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act

            User actualUser = sut.Create(testUser);

            //Assert

            Assert.AreEqual(testUser, actualUser);
        }

        [TestMethod]
        public void CreateUser_Should_ThrowException_When_ParametersAreNotValid()
        {
            //Arrange

            User testUser = new User()
            {
                Id = 1,
                FirstName = "TestFirst",
                LastName = "TestLast",
                Email = "test@gmail.com",
                Username = "testUsername",
                Password = "MTIz"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.UsernameExists(It.IsAny<string>()))
                .Returns(true);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<DuplicateEntityException>(()=>sut.Create(testUser));
        }

        [TestMethod]
        public void CreateUser_Should_ThrowException_When_AnotherParametersAreNotValid()
        {
            //Arrange

            User testUser = new User()
            {
                Id = 1,
                FirstName = "TestFirst",
                LastName = "TestLast",
                Email = "test@gmail.com",
                Username = "testUsername",
                Password = "MTIz"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.EmailExists(It.IsAny<string>()))
                .Returns(true);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<DuplicateEntityException>(() => sut.Create(testUser));
        }

        [TestMethod]
        public void DeleteUser_Should_ThrowException_When_UserIsNotCreator()
        {
            //Arrange 

            User loggedUser = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = false
            };
            User userToDelete = new User()
            {
                Id = 3,
                FirstName = "Mara",
                LastName = "Dobreva",
                Email = "m.dobreva@gmail.com",
                Username = "marcheto",
                Password = "fjsdda",
                PhoneNumber = "0797556285",
                IsAdmin = false,
                IsBlocked = false
            };

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

            Assert.ThrowsException <UnauthorizedOperationException>(() => sut.Delete(3, loggedUser));

        }

        [TestMethod]
        public void DeleteUser_Should__When_UserIsAdmin()
        {
            //Arrange 

            User loggedUser = new User()
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
            User userToDelete = new User()
            {
                Id = 3,
                FirstName = "Mara",
                LastName = "Dobreva",
                Email = "m.dobreva@gmail.com",
                Username = "marcheto",
                Password = "fjsdda",
                PhoneNumber = "0797556285",
                IsAdmin = false,
                IsBlocked = false
            };

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

            sut.Delete(3, loggedUser);

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

            List<User> users = new List<User>()
            {
                new User
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
                },

                new User
                {
                     Id = 2,
                     FirstName = "Mariq",
                     LastName = "Petrova",
                     Email = "m.petrova@gmail.com",
                     Username = "mariicheto",
                     Password = "wdljsl",
                     PhoneNumber = "0897554285",
                     IsAdmin = false,
                     IsBlocked = false
                },

                new User
                {
                     Id=3,
                     FirstName = "Mara",
                     LastName = "Dobreva",
                     Email = "m.dobreva@gmail.com",
                     Username = "marcheto",
                     Password = "fjsdda",
                     PhoneNumber = "0797556285",
                     IsAdmin = false,
                     IsBlocked = false
                }
            };

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

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.GetByUsername("ivanchoDraganchov"))
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

            User userToUpdate = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = false
            };

            User loggedUser = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = false
            };

            User user = new User()
            {
                Id = 2,
                FirstName = "Mareto",
                LastName = "Petrovka",
                Username = null
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo=>repo.GetById(2))
                .Returns(userToUpdate);
            userRepositoryMock
                .Setup(repo => repo.Update(2, user))
                .Returns(user);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act

            User updatedUser = sut.Update(2, user, loggedUser);

            //Assert

            Assert.AreEqual(user, updatedUser);
        }

        [TestMethod]
        public void UpdateUser_Should_ThrowException_When_TriesToChangeUsername()
        {
            //Arrange

            User userToUpdate = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = false
            };

            User loggedUser = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = false
            };

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
                .Setup(repo => repo.GetById(2))
                .Returns(userToUpdate);
            userRepositoryMock
                .Setup(repo => repo.Update(2, user))
                .Returns(user);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<InvalidOperationException>(() => sut.Update(2, user, loggedUser));
        }

        [TestMethod]
        public void UpdateUser_Should_ThrowException_When_UpdatorIsNotAuthorized()
        {
            //Arrange

            User userToUpdate = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = false
            };

            User loggedUser = new User()
            {
                Id = 3,
                FirstName = "Mara",
                LastName = "Dobreva",
                Email = "m.dobreva@gmail.com",
                Username = "marcheto",
                Password = "fjsdda",
                PhoneNumber = "0797556285",
                IsAdmin = false,
                IsBlocked = false
            };

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
                .Setup(repo => repo.GetById(2))
                .Returns(userToUpdate);
            userRepositoryMock
                .Setup(repo => repo.Update(2, user))
                .Returns(user);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<UnauthorizedOperationException>(() => sut.Update(2, user, loggedUser));
        }

        [TestMethod]
        public void UpdateUser_Should_ThrowException_When_EmailExists()
        {
            //Arrange

            User userToUpdate = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = false
            };

            User loggedUser = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = false
            };

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
                .Setup(repo => repo.GetById(2))
                .Returns(userToUpdate);
            userRepositoryMock
                .Setup(repo => repo.EmailExists("m.dobreva@gmail.com"))
                .Returns(true);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<DuplicateEntityException>(() => sut.Update(2, user, loggedUser));
        }

        [TestMethod]
        public void Promote_Should_ReturnUserPromoted_When_ParametersAreValid()
        {
            //Arrange

            User userToPromote = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = false
            };

            User expectedUser = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = true,
                IsBlocked = false
            };

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

            User userToBlock = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = false
            };

            User expectedUser = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = true
            };

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

            User userToBlock = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = true
            };

            User expectedUser = new User()
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = false
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.UnblockUser(userToBlock))
                .Returns(expectedUser);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            //Act

            User actualUser = sut.UnblockUser(userToBlock);

            //Assert

            Assert.AreEqual(expectedUser, actualUser);
        }

        [TestMethod]
        public void FilterBy_Should_ReturnCorrectList_When_ParametersAreValid()
        {
            //Arrange

            List<User> users = new List<User>()
            {
                new User
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
                },

                new User
                {
                     Id = 2,
                     FirstName = "Mariq",
                     LastName = "Petrova",
                     Email = "m.petrova@gmail.com",
                     Username = "mariicheto",
                     Password = "wdljsl",
                     PhoneNumber = "0897554285",
                     IsAdmin = false,
                     IsBlocked = false
                },

                new User
                {
                     Id=3,
                     FirstName = "Mara",
                     LastName = "Dobreva",
                     Email = "m.dobreva@gmail.com",
                     Username = "marcheto",
                     Password = "fjsdda",
                     PhoneNumber = "0797556285",
                     IsAdmin = false,
                     IsBlocked = false
                }
            };

            List<User> expectedUsers = new List<User>()
            {
                new User
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
                }
            };

            UserQueryParameters filterParameters = new UserQueryParameters()
            {
                FirstName = "Ivan"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            userRepositoryMock
                .Setup(repo => repo.FilterBy(filterParameters))
                .Returns(expectedUsers);

            var sut = new UserService(userRepositoryMock.Object, likeCommentRepositoryMock.Object);

            // Act

            List<User> filteredUsers = sut.FilterBy(filterParameters);

            // Assert

            Assert.AreEqual(filteredUsers, expectedUsers);
        }
    }
}