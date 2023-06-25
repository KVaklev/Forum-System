using Business.Services.Contracts;
using DataAccess.Models;
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
    public class PostServicesTests
    {
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


        //[TestMethod]
        //public void UpdatePost_ShouldReturnUpdatedPost_WhenParamsAreValid()
        //{
        //    Arrange
        //    int postId = 1;

        //    Post updatedPost = new Post()
        //    {
        //        Id = postId,
        //        Title = "Updated Title",
        //        Content = "Updated content",
        //        UserId = 2,
        //        CategoryId = 1,
        //        DateTime = DateTime.Now
        //    };

        //    var postRepositoryMock = new Mock<IPostRepository>();
        //    var tagServiceMock = new Mock<ITagService>();
        //    var likePostRepositoryMock = new Mock<ILikePostRepository>();

        //    postRepositoryMock.Setup(repo => repo.Exists(postId)).Returns(true);
        //    postRepositoryMock.Setup(repo => repo.Update(postId, updatedPost)).Returns(updatedPost);


        //    var sut = new PostService(postRepositoryMock.Object, tagServiceMock.Object, likePostRepositoryMock.Object);
        //    var sut = new PostService(postRepositoryMock.Object, tagRepositoryMock.Object, likePostRepositoryMock.Object);

        //    Act
        //   Post result = sut.UpdatePost(updatedPost);

        //    Assert
        //    Assert.AreEqual(updatedPost, result);
        //    postRepositoryMock.Verify(repo => repo.Update(updatedPost), Times.Once);
        //    postRepositoryMock.Verify(repo => repo.Exists(postId), Times.Once);
        //}

        //[TestMethod]
        //public void UpdatePost_Should_ReturnCorrectPost_When_ParametersAreValid()
        //{
        //    //Arrange

        //    Post postToUpdate = new Post()
        //    {
        //        Id = 1,
        //        Title = "Cooking Your Food",
        //        Content = "When you are able to get an accommodation that has a kitchen and cooking implements, do you cook your own food? My sister has that style. She cooks breakfast at least so they can save a little money. We once had booked in a small hotel in Hong Kong but we forgo with the cooking. For us, a vacation should be savored to the fullest.",
        //        UserId = 2,
        //        CategoryId = 1,
        //        DateTime = DateTime.Now
        //    };


        //    Post updatedPost = new Post()
        //    {
        //        Id = 1,
        //        Title = "Prepare your food"                
        //    };

        //    List<string> tagsToAdd = new List<string>()
        //    {
        //        "tag1",
        //        "tag2",
        //        "tag3"
        //    };

        //    var postRepositoryMock = new Mock<IPostRepository>();
        //    var tagServiceMock = new Mock<ITagService>();
        //    var likePostRepositoryMock = new Mock<ILikePostRepository>();

        //    postRepositoryMock
        //        .Setup(repo => repo.GetById(1))
        //        .Returns(postToUpdate);
        //    postRepositoryMock
        //        .Setup(repo => repo.Update(1, updatedPost))
        //        .Returns(updatedPost);

        //    var sut = new PostService(postRepositoryMock.Object, tagServiceMock.Object, likePostRepositoryMock.Object);

        //    //Act

        //    Post updatedPost = sut.Update(1, updatedPost);

        //    //Assert

        //    Assert.AreEqual(user, updatedUser);
        //}

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
    }
}
