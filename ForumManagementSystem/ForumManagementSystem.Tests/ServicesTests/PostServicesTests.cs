using Business.Exceptions;
using Business.Services.Contracts;
using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using ForumManagementSystem.Services;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace ForumManagementSystem.Tests.ServicesTests
{
    [TestClass]
    public class PostServicesTests
    {
        [TestMethod]
        public void GetAll_ShouldReturnAllPosts_WhenParamsAreValid()
        {
            // Arrange
            List<Post> expectedPosts = new List<Post>()
            {
                new Post()
                {
                    Id = 1,
                    Title = "Post 1",
                    Content = "Content of post 1",
                    UserId = 1,
                    CategoryId = 1,
                    DateTime = DateTime.Now
                },
                new Post()
                {
                    Id = 2,
                    Title = "Post 2",
                    Content = "Content of post 2",
                    UserId = 2,
                    CategoryId = 2,
                    DateTime = DateTime.Now
                },

                new Post()
                {
                    Id = 3,
                    Title = "Post 3",
                    Content = "Content of post 3",
                    UserId = 3,
                    CategoryId = 1,
                    DateTime = DateTime.Now
                }
            };

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.GetAll()).Returns(expectedPosts);

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            // Act
            List<Post> result = sut.GetAll();

            // Assert
            Assert.AreEqual(expectedPosts.Count, result.Count);

            for (int i = 0; i < expectedPosts.Count; i++)
            {
                Assert.AreEqual(expectedPosts[i], result[i]);
            }
            postRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);

        }

        [TestMethod]
        public void GetByPostId_ShouldReturnPost_WhenParamsAreValid()
        {
            //Arrange
            Post expectedPost = new Post()
            {

                Id = 1,
                Title = "Cooking Your Food",
                Content = "When you are able to get an accommodation that has a kitchen and cooking implements, do you cook your own food? My sister has that style. She cooks breakfast at least so they can save a little money. We once had booked in a small hotel in Hong Kong but we forgo with the cooking. For us, a vacation should be savored to the fullest.",
                UserId = 2,
                CategoryId = 1,
                DateTime = DateTime.Now
            };

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
            int userId = 2;
            Post expectedPost = new Post()
            {
                Id = 1,
                Title = "Cooking Your Food",
                Content = "When you are able to get an accommodation that has a kitchen and cooking implements, do you cook your own food? My sister has that style. She cooks breakfast at least so they can save a little money. We once had booked in a small hotel in Hong Kong but we forgo with the cooking. For us, a vacation should be savored to the fullest.",
                UserId = userId,
                CategoryId = 1,
                DateTime = DateTime.Now
            };

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(expectedPost);

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            // Act
            Post actualPost = sut.GetById(It.IsAny<int>());

            // Assert
            Assert.AreEqual(expectedPost, actualPost);
            Assert.AreEqual(userId, actualPost.UserId);
        }

        [TestMethod]
        public void GetByUser_ShouldReturnPost_WhenParamsAreValid()
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

            Post expectedPost = new Post()
            {
                Id = 1,
                Title = "Cooking Your Food",
                Content = "When you are able to get an accommodation that has a kitchen and cooking implements, do you cook your own food? My sister has that style. She cooks breakfast at least so they can save a little money. We once had booked in a small hotel in Hong Kong but we forgo with the cooking. For us, a vacation should be savored to the fullest.",
                UserId = 2,
                CategoryId = 1,
                DateTime = DateTime.Now
            };



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
            PostQueryParameters filterParameters = new PostQueryParameters
            {
                UserId = 1
            };

            List<Post> expectedPosts = new List<Post>
            {
                  new Post
                  {
                      Id = 2,
                      Title = "Cooking Your Food",
                      Content = "When you are able to get an accommodation that has a kitchen and cooking implements, do you cook your own food? My sister has that style. She cooks          breakfast   at least so they can save a little money. We once had booked in a small hotel in Hong Kong but we forgo with the cooking. For us, a  vacation    should  be     savored to  the  fullest.",
                      UserId = 1,

                  },

            };

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
            PostQueryParameters filterParameters = new PostQueryParameters
            {
                Username = "ivanchoDraganchov"
            };

            List<Post> expectedPosts = new List<Post>
            {
                  new Post
                  {
                      Id = 2,
                      Title = "Cooking Your Food",
                      Content = "When you are able to get an accommodation that has a kitchen and cooking implements, do you cook your own food? My sister has that style. She cooks          breakfast   at least so they can save a little money. We once had booked in a small hotel in Hong Kong but we forgo with the cooking. For us, a  vacation    should  be     savored to  the  fullest.",
                      UserId = 1,

                  },
            };

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
            Post testPost = new Post
            {
                Id = 1,
                Title = "Cooking Your Food",
                Content = "When you are able to get an accommodation that has a kitchen and cooking implements, do you cook your own food? My sister has that style. She cooks breakfast at least so they can save a little money. We once had booked in a small hotel in Hong Kong but we forgo with the cooking. For us, a vacation should be savored to the fullest.",
                UserId = 1
            };

            User testUser = new User
            {
                Id = 1,
                FirstName = "TestFirst",
                LastName = "TestLast",
                Email = "test@gmail.com",
                Username = "testUsername",
                Password = "MTIz"
            };

            List<string> tagsToAdd = new List<string> { "tag1", "tag2" };

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
            User existingUser = new User
            {
                Id = 1,
                FirstName = "ExistingFirst",
                LastName = "ExistingLast",
                Email = "existing@gmail.com",
                Username = "existingUser",
                IsBlocked = false,
            };

            User unauthorizedUser = new User
            {
                Id = 2,
                FirstName = "UnauthorizedFirst",
                LastName = "UnauthorizedLast",
                Email = "unauthorized@gmail.com",
                Username = "unauthorizedUser",
                IsBlocked = true

            };

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
            int postId = 1;
            User testUser = new User
            {
                Id = 1,
                FirstName = "TestFirst",
                LastName = "TestLast",
                Email = "test@gmail.com",
                Username = "testUsername",
                Password = "MTIz"
            };

            Post existingPost = new Post
            {
                Id = postId,
                Title = "Existing Title",
                Content = "Existing Content",
                CreatedBy = testUser
            };

            Post updatedPost = new Post
            {
                Id = postId,
                Title = "Updated Title",
                Content = "Updated Content",
                CreatedBy = testUser
            };

            List<string> tagsToAdd = new List<string> { "tag1", "tag2" };

            var postRepositoryMock = new Mock<IPostRepository>();
            var tagRepositoryMock = new Mock<ITagService>();
            var likePostRepositoryMock = new Mock<ILikePostRepository>();

            postRepositoryMock.Setup(repo => repo.GetById(postId)).Returns(existingPost);
            postRepositoryMock.Setup(repo => repo.TitleExists(updatedPost.Title)).Returns(true);

            var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

            // Act and Assert
            Assert.ThrowsException<DuplicateEntityException>(() => sut.Update(postId, updatedPost, testUser, tagsToAdd));
        }


        [TestMethod]
        public void CreatePost_ShouldReturnNewPost_WhenParamsAreValid()
        {
            // Arrange
            Post newPost = new Post()
            {
                Id = 1,
                Title = "Cooking Your Food",
                Content = "When you are able to get an accommodation that has a kitchen and cooking implements, do you cook your own food? My sister has that style. She cooks breakfast at least so they can save a little money. We once had booked in a small hotel in Hong Kong but we forgo with the cooking. For us, a vacation should be savored to the fullest.",
                UserId = 2,
                CategoryId = 1,
                DateTime = DateTime.Now
            };

            User testUser = new User()
            {
                Id = 1,
                FirstName = "TestFirst",
                LastName = "TestLast",
                Email = "test@gmail.com",
                Username = "testUsername",
                Password = "MTIz"
            };

            List<string> tagsToAdd = new List<string>()
            {
                "tag1",
                "tag2",
                "tag3"
            };

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
            postRepositoryMock.Verify(repo => repo.Create(newPost, testUser), Times.Once);
            foreach (var tagName in tagsToAdd)
            {
                tagServiceMock.Verify(tagService => tagService.Create(tagName), Times.Once);
            }
        }


        //    [TestMethod]
        //    public void UpdatePost_Should_ReturnCorrectPost_When_ParametersAreValid()
        //    {
        //        // Arrange
        //        int postId = 1;

        //        Post postToUpdate = new Post()
        //        {
        //            Id = postId,
        //            Title = "Cooking Your Food",
        //            Content = "When you are able to get an accommodation that has a kitchen and cooking implements, do you cook your own food? My sister has that style. She cooks breakfast at least so they can save a little money. We once had booked in a small hotel in Hong Kong but we forgo with the cooking. For us, a vacation should be savored to the fullest.",
        //            UserId = 2,
        //            CategoryId = 1,
        //            DateTime = DateTime.Now
        //        };

        //        Post updatedPost = new Post()
        //        {
        //            Id = postId,
        //            Title = "Prepare your food"
        //        };

        //        User testUser = new User()
        //        {
        //            Id = 1,
        //            FirstName = "TestFirst",
        //            LastName = "TestLast",
        //            Email = "test@gmail.com",
        //            Username = "testUsername",
        //            Password = "MTIz"
        //        };

        //        List<string> tagsToAdd = new List<string>()
        //        {
        //            "tag1",
        //            "tag2",
        //            "tag3"
        //        };

        //        var postRepositoryMock = new Mock<IPostRepository>();
        //        var tagServiceMock = new Mock<ITagService>();
        //        var likePostRepositoryMock = new Mock<ILikePostRepository>();
        //        var authorizationServiceMock = new Mock<IAuthorizationService>();

        //        postRepositoryMock.Setup(repo => repo.GetById(postId)).Returns(postToUpdate);
        //        postRepositoryMock.Setup(repo => repo.Update(postId, It.IsAny<Post>())).Returns(updatedPost);
        //        authorizationServiceMock.Setup(service => service.IsAuthorized(postToUpdate.CreatedBy, testUser)).Returns(true);

        //        var sut = new PostService(postRepositoryMock.Object, tagServiceMock.Object, likePostRepositoryMock.Object, authorizationServiceMock.Object);

        //        // Act
        //        sut.Update(postId, updatedPost, testUser, tagsToAdd);

        //        // Assert
        //        Assert.AreEqual(postToUpdate, updatedPost);
        //        postRepositoryMock.Verify(repo => repo.GetById(postId), Times.Once);
        //        postRepositoryMock.Verify(repo => repo.Update(postId, It.IsAny<Post>()), Times.Once);
        //        authorizationServiceMock.Verify(service => service.IsAuthorized(postToUpdate.CreatedBy, testUser), Times.Once);
        //    }

        //}
    }
}
