using Business.Services.Contracts;
using Business.Services.Models;
using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using ForumManagementSystem.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ForumManagementSystem.Tests.ServicesTests
{
    [TestClass]
    public class LikePostServicesTests
    {
        [TestMethod]
        public void Update_ShouldAddNewLike_When_NotLiked()
        {
            User testUser = TestHelpers.GetTestUser();
            Post testPost = TestHelpers.GetTestPost();
            LikePost testLikePost = TestHelpers.GetLikePostIsLiked();


            var likePostRepository = new Mock<ILikePostRepository>();

            likePostRepository
                .Setup(repo => repo.Get(testPost, testUser))
                .Throws(new EntityNotFoundException($"This post is not liked."));

            likePostRepository
                .Setup(repo => repo.Create(testPost, testUser))
                .Returns(testLikePost);

            var sut = new LikePostService(likePostRepository.Object);

            LikePost result = sut.Update(testPost, testUser);

        }

        [TestMethod]
        public void Update_When_Liked()
        {
            User testUser = TestHelpers.GetTestUser();
            Post testPost = TestHelpers.GetTestPost();
            LikePost testLikePost = TestHelpers.GetLikePostIsLiked();
            LikePost testUpdateLikePost = TestHelpers.GetLikePostIsNotLiked();

            var likePostRepository = new Mock<ILikePostRepository>();

            likePostRepository
                .Setup(repo => repo.Get(testPost, testUser))
                .Returns(testLikePost);

            likePostRepository
                .Setup(repo => repo.Update(testPost, testUser))
                .Returns(testUpdateLikePost);

            var sut = new LikePostService(likePostRepository.Object);

            LikePost result = sut.Update(testPost, testUser);

            Assert.AreEqual(testUpdateLikePost, result);


        }
    }
}
