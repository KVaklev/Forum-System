//using Business.Exceptions;
//using DataAccess.Repositories.Contracts;
//using ForumManagementSystem.Models;
//using ForumManagementSystem.Repository;
//using ForumManagementSystem.Services;
//using ForumManagementSystem.Tests.Helpers;
//using Moq;

//namespace ForumManagementSystem.Tests.ServicesTests
//{
//    [TestClass]
//    public class CommentServicesTests
//    {
//        [TestMethod]
//        public void GetById_Should_ReturnCorrectComment_When_ParametersAreValid()
//        {
//            //Arrange

//            Comment expectedComment = TestHelpers.GetTestComment();

//            var commentRepositoryMock = new Mock<ICommentRepository>();
//            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();
//            var categoryRepositoryMock = new Mock<ICategoryRepository>();
//            var postRepositoryMock = new Mock<IPostRepository>();

//            commentRepositoryMock
//                .Setup(repo => repo.GetByID(expectedComment.Id))
//                .Returns(expectedComment);

//            var sut = new CommentService(
//                commentRepositoryMock.Object, 
//                likeCommentRepositoryMock.Object,
//                categoryRepositoryMock.Object,
//                postRepositoryMock.Object);

//            //Act

//            Comment actualComment = sut.GetByID(expectedComment.Id);

//            //Assert

//            Assert.AreEqual(expectedComment, actualComment);

//        }

//        //[TestMethod]
//        //public void FilterBy_Should_ReturnCorrectList_When_ParametersAreValid()
//        //{
//        //    //Arrange

//        //    List<Comment> comments = TestHelpers.GetTestListComments();

//        //    List<Comment> expectedComment = TestHelpers.GetTestExpectedListComments();

//        //    CommentQueryParameters filterParameters = new CommentQueryParameters()
//        //    {
//        //        UserId = 1,
//        //    };

//        //    var commentRepositoryMock = new Mock<ICommentRepository>();
//        //    var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();
//        //    var categoryRepositoryMock = new Mock<ICategoryRepository>();
//        //    var postRepositoryMock = new Mock<IPostRepository>();

//        //    commentRepositoryMock
//        //        .Setup(repo => repo.FilterBy(filterParameters))
//        //        .Returns(expectedComment);

//        //    var sut = new CommentService(
//        //        commentRepositoryMock.Object, 
//        //        likeCommentRepositoryMock.Object,
//        //        categoryRepositoryMock.Object,
//        //        postRepositoryMock.Object);

//        //    // Act

//        //    List<Comment> filteredUsers = sut.FilterBy(filterParameters);

//        //    // Assert

//        //    Assert.AreEqual(filteredUsers, expectedComment);
//        //}

//        [TestMethod]
//        public void CreateComment_Should_ReturnCorrectComment_When_ParametersAreValid()
//        {
//            //Arrange

//            Comment expectedComment = TestHelpers.GetTestComment();

//            User loggedUser = TestHelpers.GetTestUserAdmin();

//            var commentRepositoryMock = new Mock<ICommentRepository>();
//            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();
//            var categoryRepositoryMock = new Mock<ICategoryRepository>();
//            var postRepositoryMock = new Mock<IPostRepository>();

//            commentRepositoryMock
//                .Setup(repo => repo.Create(expectedComment, loggedUser))
//                .Returns(expectedComment);

//            var sut = new CommentService(
//                commentRepositoryMock.Object, 
//                likeCommentRepositoryMock.Object,
//                categoryRepositoryMock.Object,
//                postRepositoryMock.Object);

//            //Act

//            Comment actualComment = sut.Create(expectedComment, loggedUser);

//            //Assert

//            Assert.AreEqual(expectedComment, actualComment);
//        }

//        [TestMethod]
//        public void CreateComment_Should_ThrowException_When_UserIsBlocked()
//        {
//            //Arrange

//            Comment expectedComment = TestHelpers.GetTestComment();

//            User loggedUser = TestHelpers.GetTestExpectedUserAsBlocked();

//            var commentRepositoryMock = new Mock<ICommentRepository>();
//            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();
//            var categoryRepositoryMock = new Mock<ICategoryRepository>();
//            var postRepositoryMock = new Mock<IPostRepository>();

//            commentRepositoryMock
//                .Setup(repo => repo.Create(expectedComment, loggedUser))
//                .Throws(new UnauthorizedOperationException("Blocked user cannot create comment."));

//            var sut = new CommentService(
//                commentRepositoryMock.Object, 
//                likeCommentRepositoryMock.Object,
//                categoryRepositoryMock.Object,
//                postRepositoryMock.Object);

//            //Act & Assert
//            Assert.ThrowsException<UnauthorizedOperationException>(() => sut.Create(expectedComment, loggedUser));
//        }

//        [TestMethod]
//        public void UpdateComment_Should_ReturnCorrectComment_When_ParametersAreValid()
//        {
//            //Arrange

//            Comment commentToUpdate = TestHelpers.GetTestUpdateComment();

//            User loggedUser = TestHelpers.GetTestUserAdmin();

//            var commentRepositoryMock = new Mock<ICommentRepository>();
//            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();
//            var categoryRepositoryMock = new Mock<ICategoryRepository>();
//            var postRepositoryMock = new Mock<IPostRepository>();

//            commentRepositoryMock
//                .Setup(repo => repo.GetByID(commentToUpdate.Id))
//                .Returns(commentToUpdate);
//            commentRepositoryMock
//                .Setup(repo => repo.Update(commentToUpdate.Id, commentToUpdate))
//                .Returns(commentToUpdate);

//            var sut = new CommentService(
//                commentRepositoryMock.Object, 
//                likeCommentRepositoryMock.Object,
//                categoryRepositoryMock.Object,
//                postRepositoryMock.Object);

//            //Act

//            Comment updatedComment = sut.Update(commentToUpdate.Id, commentToUpdate, loggedUser);

//            //Assert

//            Assert.AreEqual(commentToUpdate, updatedComment);
//        }

//        [TestMethod]
//        public void UpdateComment_Should_ThrowException_When_UpdatorIsNotAuthorized()
//        {
//            Comment commentToUpdate = TestHelpers.GetTestUpdateComment();

//            User loggedUser = TestHelpers.GetTestCreateUser();

//            var commentRepositoryMock = new Mock<ICommentRepository>();
//            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();
//            var categoryRepositoryMock = new Mock<ICategoryRepository>();
//            var postRepositoryMock = new Mock<IPostRepository>();

//            commentRepositoryMock
//                .Setup(repo => repo.GetByID(commentToUpdate.Id))
//                .Returns(commentToUpdate);
//            commentRepositoryMock
//                .Setup(repo => repo.Update(commentToUpdate.Id, commentToUpdate))
//                .Returns(commentToUpdate);

//            var sut = new CommentService(
//                commentRepositoryMock.Object, 
//                likeCommentRepositoryMock.Object,
//                categoryRepositoryMock.Object,
//                postRepositoryMock.Object);

//            //Act & Assert

//            Assert.ThrowsException<UnauthorizedOperationException>(() => sut.Update(commentToUpdate.Id, commentToUpdate, loggedUser));
//        }

//        [TestMethod]
//        public void UpdateComment_Should_ThrowException_When_UpdatorIsBlocked()
//        {
//            //Arrange

//            Comment commentToUpdate = TestHelpers.GetTestUpdateComment();

//            User loggedUser = TestHelpers.GetTestExpectedUserAsBlocked();

//            var commentRepositoryMock = new Mock<ICommentRepository>();
//            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();
//            var categoryRepositoryMock = new Mock<ICategoryRepository>();
//            var postRepositoryMock = new Mock<IPostRepository>();

//            commentRepositoryMock
//                .Setup(repo => repo.GetByID(commentToUpdate.Id))
//                .Returns(commentToUpdate);
//            commentRepositoryMock
//                .Setup(repo => repo.Update(commentToUpdate.Id, commentToUpdate))
//                .Returns(commentToUpdate);

//            var sut = new CommentService(
//                commentRepositoryMock.Object, 
//                likeCommentRepositoryMock.Object,
//                categoryRepositoryMock.Object,
//                postRepositoryMock.Object);

//            //Act & Assert

//            Assert.ThrowsException<UnauthorizedOperationException>(() => sut.Update(commentToUpdate.Id, commentToUpdate, loggedUser));
//        }

//        [TestMethod]
//        public void DeleteComment_Should__When_UserIsAdmin()
//        {
//            //Arrange 

//            User loggedUser = TestHelpers.GetTestUserAdmin();

//            Comment commentToDelete = TestHelpers.GetTestComment();

//            var commentRepositoryMock = new Mock<ICommentRepository>();
//            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();
//            var categoryRepositoryMock = new Mock<ICategoryRepository>();
//            var postRepositoryMock = new Mock<IPostRepository>();

//            commentRepositoryMock
//                .Setup(repo => repo.GetByID(commentToDelete.Id))
//                .Returns(commentToDelete);
//            likeCommentRepositoryMock
//                .Setup(repo => repo.DeleteByComment(commentToDelete))
//                .Returns(0);
//            commentRepositoryMock
//                .Setup(repo => repo.Delete(commentToDelete.Id))
//                .Returns(commentToDelete);

//            var sut = new CommentService(
//                commentRepositoryMock.Object, 
//                likeCommentRepositoryMock.Object,
//                categoryRepositoryMock.Object,
//                postRepositoryMock.Object
//                );

//            //Act 

//            sut.Delete(commentToDelete.Id, loggedUser);

//            //Assert

//            commentRepositoryMock
//                .Verify(repo => repo.Delete(commentToDelete.Id), Times.Once);
//        }

//        [TestMethod]
//        public void DeleteComment_Should_ThrowException_When_UserIsNotAdmin()
//        {
//            //Arrange 

//            Comment commentToDelete = TestHelpers.GetTestComment();

//            User loggedUser = TestHelpers.GetTestCreateUser();

//            var commentRepositoryMock = new Mock<ICommentRepository>();
//            var likeCommentRepositoryMock = new Mock<ILikeCommentRepository>();
//            var categoryRepositoryMock = new Mock<ICategoryRepository>();
//            var postRepositoryMock = new Mock<IPostRepository>();

//            commentRepositoryMock
//                .Setup(repo => repo.GetByID(commentToDelete.Id))
//                .Returns(commentToDelete);
//            likeCommentRepositoryMock
//               .Setup(repo => repo.DeleteByComment(commentToDelete))
//               .Returns(0);
//            commentRepositoryMock
//                .Setup(repo => repo.Delete(commentToDelete.Id))
//                .Returns(commentToDelete);

//            var sut = new CommentService(
//                commentRepositoryMock.Object, 
//                likeCommentRepositoryMock.Object,
//                categoryRepositoryMock.Object,
//                postRepositoryMock.Object);

//            //Act & Assert

//            Assert.ThrowsException<UnauthorizedOperationException>(() => sut.Delete(commentToDelete.Id, loggedUser));

//        }

//    }
//}