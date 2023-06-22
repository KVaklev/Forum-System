using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using ForumManagementSystem.Services;
using Moq;

namespace ForumManagementSystem.Tests.ServicesTests
{
    [TestClass]
    public class UserServicesTests
    {
        [TestMethod]
        public void GetById_ShouldReturnCorrectUser_When_ParametersAreValid()
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
    }
}