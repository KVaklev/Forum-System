using Business.Exceptions;
using Business.Services.Models;
using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Tests.Helpers;
using Moq;

namespace ForumManagementSystem.Tests.ServicesTests
{
    [TestClass]
    public class TagServicesTests
    {
        [TestMethod]
        public void GetById_Should_ReturnCorrectUser_When_ParametersAreValid()
        {
            //Arrange

            Tag expectedTag = TestHelpers.GetTestTag();

            var tagRepositoryMock = new Mock<ITagRepository>();
         
            tagRepositoryMock
                .Setup(repo => repo.GetById(expectedTag.Id))
                .Returns(expectedTag);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act

            Tag actualTag = sut.GetById(expectedTag.Id);

            //Assert

            Assert.AreEqual(expectedTag, actualTag);

        }

        [TestMethod]
        public void GetByName_Should_ReturnCorrectTag_When_ParametersAreValid()
        {
            //Arrange

            Tag expectedTag = TestHelpers.GetTestTag();

            var tagRepositoryMock = new Mock<ITagRepository>();

            tagRepositoryMock
                .Setup(repo => repo.GetByName(expectedTag.Name))
                .Returns(expectedTag);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act

            Tag actualTag = sut.GetByName(expectedTag.Name);

            //Assert

            Assert.AreEqual(expectedTag, actualTag);

        }

        [TestMethod]
        public void GetAll_Should_ReturnCorrectList_When_ParametersAreValid()
        {
            //Arrange

            List<Tag> tags = TestHelpers.GetTestListTags();

            var tagRepositoryMock = new Mock<ITagRepository>();

            tagRepositoryMock
                .Setup(repo => repo.GetAll())
                .Returns(tags);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act

            var expectedTags = sut.GetAll();

            //Assert

            Assert.AreEqual(expectedTags, tags);
        }

        [TestMethod]
        public void CreateTag_Should_ReturnCorrectTag_When_ParametersAreValid()
        {
            //Arrange

            Tag testTag = TestHelpers.GetTestTag();

            User loggedUser = TestHelpers.GetTestUser();

            var tagRepositoryMock = new Mock<ITagRepository>();

            tagRepositoryMock
                .Setup(repo => repo.GetByName(testTag.Name))
                .Returns(testTag);

            tagRepositoryMock
                .Setup(repo => repo.Create(testTag))
                .Returns(testTag);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act

            Tag actualTag = sut.Create(testTag.Name);

            //Assert

            Assert.AreEqual(testTag, actualTag);
        }

        [TestMethod]
        public void CreateTag_Should_ReturnAnotherCorrectTag_When_ParametersAreValid()
        {
            //Arrange

            Tag testTag = TestHelpers.GetTestTag();

            User loggedUser = TestHelpers.GetTestUser();

            var tagRepositoryMock = new Mock<ITagRepository>();

            tagRepositoryMock
                .Setup(repo => repo.GetByName(testTag.Name))
                .Throws(new EntityNotFoundException("Tag name doesn't exist."));

            tagRepositoryMock
                .Setup(repo => repo.Create(testTag))
                .Callback<Tag>(tag => testTag = tag);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act

            Tag actualTag = sut.Create(testTag.Name);

            //Assert

            Assert.AreEqual(testTag.Name, actualTag.Name);
        }

        [TestMethod]
        public void EditTag_Should_ThrowException_When_LoggedUserIsNotAuthorized()
        {
            //Arrange

            Tag existingTag = TestHelpers.GetTestTag();

            User loggedUser = TestHelpers.GetTestUser();

            Tag newTag = TestHelpers.GetTestEditTag();

            var tagRepositoryMock = new Mock<ITagRepository>();

            tagRepositoryMock
                .Setup(repo => repo.GetById(existingTag.Id))
                .Returns(existingTag);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<UnauthorizedOperationException>(() => sut.Edit(existingTag.Id, newTag, loggedUser));
        }

        [TestMethod]
        public void EditTag_Should_ThrowException_When_TagWithNameExists()
        {
            //Arrange

            Tag existingTag = TestHelpers.GetTestTag();

            Tag newTag = TestHelpers.GetTestTag();

            User loggedUser = TestHelpers.GetTestUserAdmin();

            var tagRepositoryMock = new Mock<ITagRepository>();

            tagRepositoryMock
                .Setup(repo => repo.GetById(existingTag.Id))
                .Returns(existingTag);
            tagRepositoryMock
                .Setup(repo=>repo.NameExists(newTag.Name))
                .Returns(true);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<DuplicateEntityException>(() => sut.Edit(existingTag.Id, newTag, loggedUser));
        }

        [TestMethod]
        public void DeleteTag_Should__When_UserIsAdmin()
        {
            //Arrange 

            User loggedUser = TestHelpers.GetTestUserAdmin();

            Tag tagToDelete = TestHelpers.GetTestTag();
               
            var tagRepositoryMock = new Mock<ITagRepository>();
           
            tagRepositoryMock
                .Setup(repo => repo.GetById(tagToDelete.Id))
                .Returns(tagToDelete);
           
            tagRepositoryMock
                .Setup(repo => repo.Delete(tagToDelete.Id))
                .Returns(tagToDelete);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act 

            sut.Delete(tagToDelete.Id, loggedUser);

            //Assert

            tagRepositoryMock
                .Verify(repo => repo.Delete(tagToDelete.Id), Times.Once);
        }

        [TestMethod]
        public void DeleteTag_Should_ThrowException_When_UserIsNotAdmin()
        {
            //Arrange 

            User loggedUser = TestHelpers.GetTestUser();

            Tag tagToDelete = TestHelpers.GetTestTag();

            var tagRepositoryMock = new Mock<ITagRepository>();

            tagRepositoryMock
                .Setup(repo => repo.GetById(tagToDelete.Id))
                .Returns(tagToDelete);

            tagRepositoryMock
                .Setup(repo => repo.Delete(tagToDelete.Id))
                .Returns(tagToDelete);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException <UnauthorizedOperationException>(() => sut.Delete(tagToDelete.Id, loggedUser));
        }

        [TestMethod]
        public void EditTag_Should_When_ParametersAreValid()
        {
            //Arrange

            Tag existingTag = TestHelpers.GetTestTag();

            User loggedUser = TestHelpers.GetTestUserAdmin();

            Tag editedTag = TestHelpers.GetTestEditTag();

            var tagRepositoryMock = new Mock<ITagRepository>();

            tagRepositoryMock
                .Setup(repo=>repo.GetById(existingTag.Id))
                .Returns(existingTag);
            tagRepositoryMock
                .Setup(repo => repo.GetByName(editedTag.Name))
                .Returns(existingTag);
            tagRepositoryMock
                .Setup(repo => repo.Edit(existingTag.Id, editedTag))
                .Returns(editedTag);

            var tagService = new TagService(tagRepositoryMock.Object);

            //Act
            Tag result = tagService.Edit(existingTag.Id, editedTag, loggedUser);

            //Assert
            Assert.AreEqual(editedTag, result);
        }
    }
}
