using Business.Exceptions;
using Business.Services.Contracts;
using Business.Services.Helpers;
using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Security.AccessControl;
using System.Web.Http.Services;
using ForumManagementSystem.Tests.Helpers;

namespace ForumManagementSystem.Tests.ServicesTests
{
    [TestClass]
    public class PostServicesTests
    {
        [TestMethod]
        public void GetAll_ShouldReturnAllPosts_WhenParamsAreValid()
        {
            // Arrange

            List<Post> expectedPosts = TestHelpers.GetTestListPost();

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.GetAll()).Returns(expectedPosts);

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            // Act
            List<Post> result = sut.GetAll();

            // Assert
            Assert.AreEqual(expectedPosts, result);
        }

        [TestMethod]
        public void GetByPostId_ShouldReturnPost_WhenParamsAreValid()
        {
            //Arrange
            Post expectedPost = TestHelpers.GetTestPost();

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(expectedPost);

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            //Act
            Post actualPost = sut.GetById(It.IsAny<int>());

            //Assert
            Assert.AreEqual(expectedPost, actualPost);

        }

        [TestMethod]
        public void GetById_ShouldThrowException_WhenParamsAreNotValid()
        {
            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Throws(new EntityNotFoundException("Post does not exist"));

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            Assert.ThrowsException<EntityNotFoundException>(() => sut.GetById(It.IsAny<int>()));
        }

        [TestMethod]
        public void CheckIfReturnedPostHasExpectedUser_ShouldBeTrue_WhenParamsAreValid()
        {
            // Arrange
            Post expectedPost = TestHelpers.GetTestPost();

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(expectedPost);

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            // Act
            Post actualPost = sut.GetById(It.IsAny<int>());

            // Assert
            Assert.AreEqual(expectedPost.UserId, actualPost.UserId);
        }

        [TestMethod]
        public void GetByUser_ShouldReturnPost_WhenParamsAreValid()
        {
            //Arrange
            User testUser = TestHelpers.GetTestUser();
            Post expectedPost = TestHelpers.GetTestPost();

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.GetByUser(testUser)).Returns(expectedPost);

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            //Act
            Post actualPost = sut.GetByUser(testUser);

            //Assert
            Assert.AreEqual(expectedPost, actualPost);

        }

        [TestMethod]
        public void FilterBy_ShouldReturnFilteredPosts_WhenFilterUserIdParameterAreValid()
        {
            // Arrange
            PostQueryParameters filterParameters = TestHelpers.GetUserIdAsQueryParam();

            List<Post> expectedPosts = TestHelpers.GetTestListPost();

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.FilterBy(filterParameters)).Returns(expectedPosts);

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            // Act
            List<Post> actualPosts = sut.FilterBy(filterParameters);

            // Assert
            Assert.AreEqual(expectedPosts, actualPosts);

        }

        [TestMethod]
        public void FilterBy_ShouldReturnFilteredPosts_WhenFilterUsernameParameterAreValid()
        {
            // Arrange
            PostQueryParameters filterParameters = TestHelpers.GetUsernameQueryParam();

            List<Post> expectedPosts = TestHelpers.GetTestListPost();

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.FilterBy(filterParameters)).Returns(expectedPosts);

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            // Act
            List<Post> actualPosts = sut.FilterBy(filterParameters);

            // Assert
            Assert.AreEqual(expectedPosts, actualPosts);
        }

        [TestMethod]
        public void Create_ShouldThrowDuplicateEntityException_WhenPostTitleExists()
        {
            // Arrange
            Post testPost = TestHelpers.GetTestPost();

            User testUser = TestHelpers.GetTestUser();

            List<string> tagsToAdd = TestHelpers.GetTestListTag();

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.TitleExists(testPost.Title)).Returns(true);

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            // Act and Assert
            Assert.ThrowsException<DuplicateEntityException>(() => sut.Create(testPost, testUser, tagsToAdd));
        }

        [TestMethod]
        public void Update_ShouldThrowUnauthenticatedOperationException_WhenNotAuthorized()
        {
            // Arrange
            User existingUser = TestHelpers.GetTestExpectedUserAsUnblocked();

            User unauthorizedUser = TestHelpers.GetTestExpectedUserAsBlocked();

            Post existingPost = new Post
            {
                Id = 1,
                Title = "Existing Title",
                Content = "Existing Content",
                CreatedBy = existingUser
            };

            Post updatedPost = new Post
            {
                Id = 1,
                Title = "Updated Title",
                Content = "Updated Content",
                CreatedBy = unauthorizedUser
            };

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.GetById(1)).Returns(existingPost);

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            // Act and Assert
            Assert.ThrowsException<UnauthenticatedOperationException>(() => sut.Update(1, updatedPost, unauthorizedUser, null));
        }

        [TestMethod]
        public void Update_ShouldThrowDuplicateEntityException_WhenPostTitleExists()
        {
            // Arrange
            User testUser = TestHelpers.GetTestUser();

            Post existingPost = new Post
            {
                Id = 1,
                Title = "Existing Title",
                Content = "Existing Content",
                CreatedBy = testUser
            };

            Post updatedPost = new Post
            {
                Id = 1,
                Title = "Updated Title",
                Content = "Updated Content",
                CreatedBy = testUser
            };

            List<string> tagsToAdd = TestHelpers.GetTestListTag();

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.GetById(1)).Returns(existingPost);
            postRepositoryMock.Setup(repo => repo.TitleExists(updatedPost.Title)).Returns(true);

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            // Act and Assert
            Assert.ThrowsException<DuplicateEntityException>(() => sut.Update(1, updatedPost, testUser, tagsToAdd));
        }

        [TestMethod]
        public void Update_ValidUpdate_ReturnsUpdatedPost()
        {
            // Arrange
            var repository = new Mock<IPostRepository>();
            var tagService = new Mock<ITagService>();
            var likePostRepository = new Mock<ILikePostRepository>();

            repository.Setup(r => r.GetById(It.IsAny<int>())).Returns(new Post { Id = 1, Title = "Existing Post", CreatedBy = new User() });
            repository.Setup(r => r.TitleExists(It.IsAny<string>())).Returns(false);
            repository.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<Post>())).Returns(new Post { Id = 1, Title = "Updated Post", CreatedBy = new User() });


            var post = new Post { Id = 1, Title = "Updated Post", CreatedBy = new User() };
            var loggedUser = new User() { IsAdmin = true };

            var sut = new PostService(repository.Object, tagService.Object, likePostRepository.Object);

            // Act
            var result = sut.Update(1, post, loggedUser, new List<string>());

            // Assert
            Assert.AreEqual(post.Title, result.Title);
        }

        [TestMethod]
        public void Update_TagsToAdd_TagsAddedToPost()
        {
            // Arrange
            var repository = new Mock<IPostRepository>();
            var tagService = new Mock<ITagService>();
            var likePostRepository = new Mock<ILikePostRepository>();

            var existingPost = new Post { Id = 1, Title = "Existing Post", CreatedBy = new User() };
            var updatedPost = new Post { Id = 1, Title = "Updated Post", CreatedBy = new User() };

            repository.Setup(r => r.GetById(It.IsAny<int>())).Returns(existingPost);
            repository.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<Post>())).Returns(updatedPost);

            var tagsToAdd = new List<string> { "Tag1", "Tag2", "Tag3" };

            var loggedUser = new User { IsAdmin = true };

            var sut = new PostService(repository.Object, tagService.Object, likePostRepository.Object);

            tagService.Setup(t => t.Create(It.IsAny<string>())).Returns(new Tag { Id = 1 });

            // Act
            var result = sut.Update(1, updatedPost, loggedUser, tagsToAdd);

            // Assert
            foreach (var tagName in tagsToAdd)
            {
                tagService.Verify(t => t.Create(tagName), Times.Once);
            }

            repository.Verify(r => r.AddTagToPost(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(tagsToAdd.Count));
        }

        [TestMethod]
        public void CreatePost_ShouldReturnNewPost_WhenParamsAreValid()
        {
            // Arrange
            Post newPost = TestHelpers.GetTestPost();

            User testUser = TestHelpers.GetTestUser();

            List<string> tagsToAdd = TestHelpers.GetTestListTag();

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagServiceMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.Create(newPost, testUser)).Returns(newPost);

            foreach (var tagName in tagsToAdd)
            {
                Tag newTag = new Tag { Id = 1, Name = tagName };
                tagServiceMock.Setup(tagService => tagService.Create(tagName)).Returns(newTag);
                postRepositoryMock.Setup(repo => repo.AddTagToPost(newTag.Id, newPost.Id));
            }

            var sut = new PostService(postRepositoryMock.Object, tagServiceMock.Object, likePostRepositoryMock.Object);

            // Act
            Post createdPost = sut.Create(newPost, testUser, tagsToAdd);

            // Assert
            Assert.AreEqual(newPost, createdPost);
        }

        [TestMethod]
        public void Create_PostWithNoTags_ReturnsCreatedPost()
        {
            // Arrange
            Post newPost = TestHelpers.GetTestPost();

            User testUser = TestHelpers.GetTestUser();

            List<string> tagsToAdd = null;

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagServiceMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.Create(newPost, testUser)).Returns(newPost);

            var sut = new PostService(postRepositoryMock.Object, tagServiceMock.Object, likePostRepositoryMock.Object);

            // Act
            Post createdPost = sut.Create(newPost, testUser, tagsToAdd);

            // Assert
            Assert.AreEqual(newPost, createdPost);
            postRepositoryMock.Verify(repo => repo.Create(newPost, testUser), Times.Once);

        }

        [TestMethod]
        public void Delete_PostShouldBeDeleted_WhenUserIsAuthorized()
        {
            // Arrange

            var loggedUser = TestHelpers.GetTestUser();

            var postRepository = new Mock<IPostRepository>();
            var tagsRepository = new Mock<ITagService>();
            var likePostRepository = new Mock<ILikePostRepository>();

            var postToDelete = new Post { Id = 1, CreatedBy = loggedUser };

            postRepository.Setup(r => r.GetById(1)).Returns(postToDelete);

            var postService = new PostService(postRepository.Object, tagsRepository.Object, likePostRepository.Object);

            // Act
            postService.Delete(1, loggedUser);

        }

        [TestMethod]
        public void Delete_ShouldThrowException_WhenUserIsNotAuthorized()
        {
            // Arrange
            var loggedUser = TestHelpers.GetTestUser();

            var postRepository = new Mock<IPostRepository>();
            var tagsRepository = new Mock<ITagService>();
            var likePostRepository = new Mock<ILikePostRepository>();

            var postToDelete = new Post { Id = 1, CreatedBy = new User { Id = 3 } };

            postRepository.Setup(r => r.GetById(1)).Returns(postToDelete);

            var sut = new PostService(postRepository.Object, tagsRepository.Object, likePostRepository.Object);

            // Act and Assert
            Assert.ThrowsException<UnauthenticatedOperationException>(() => sut.Delete(1, loggedUser));
        }

        [TestMethod]
        public void ShouldThrowException_WhenUserIsBlocked()
        {
            var blockedUser = TestHelpers.GetTestExpectedUserAsBlocked();

            var postRepository = new Mock<IPostRepository>();
            var tagsRepository = new Mock<ITagService>();
            var likePostRepository = new Mock<ILikePostRepository>();

            var sut = new PostService(postRepository.Object, tagsRepository.Object, likePostRepository.Object);

            Assert.ThrowsException<UnauthorizedAccessException>(() => sut.CheckIfBlocked(blockedUser));
        }
    }
}




