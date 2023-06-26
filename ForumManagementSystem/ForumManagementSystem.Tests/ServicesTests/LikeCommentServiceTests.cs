using Business.Services.Models;
using DataAccess.Models;
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
    public class LikeCommentServiceTests
    {
        [TestMethod]
        public void UpdateLikeComment_Should_When_LikeNoExist()
        {
            //Arrange
            LikeComment testLikeComment = TestHelpers.GetLikeCommentIsLiked();
            Comment testComment = TestHelpers.GetTestComment();
            User testUser = TestHelpers.GetTestUserAdmin();

            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            likeCommentRepositoryMock
                .Setup(repo => repo.Get(testComment, testUser))
                .Throws(new EntityNotFoundException($"This comment is not liked from this user."));
            likeCommentRepositoryMock
                .Setup(repo => repo.Create(testComment, testUser))
                .Returns(testLikeComment);

            var sut = new LikeCommentService(likeCommentRepositoryMock.Object);

            //Act

            LikeComment updatedLike = sut.Update(testComment, testUser);

            //Assert

            Assert.AreEqual(testLikeComment, updatedLike);
        }
        [TestMethod]
        public void UpdateLikeComment_Should_When_LikeExist()
        {
            //Arrange
            LikeComment testLikeComment = TestHelpers.GetLikeCommentIsLiked();
            LikeComment testUpdateLikeComment = TestHelpers.GetLikeCommentIsNoLiked();
            Comment testComment = TestHelpers.GetTestComment();
            User testUser = TestHelpers.GetTestUserAdmin();

            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();

            likeCommentRepositoryMock
                .Setup(repo => repo.Get(testComment, testUser))
                .Returns(testLikeComment);
            likeCommentRepositoryMock
                .Setup(repo => repo.Update(testComment, testUser))
                .Returns(testUpdateLikeComment);

            var sut = new LikeCommentService(likeCommentRepositoryMock.Object);

            //Act

            LikeComment updatedLike = sut.Update(testComment, testUser);

            //Assert

            Assert.AreEqual(testUpdateLikeComment, updatedLike);
        }
    }
}
