using Business.Exceptions;
using Business.Services.Models;
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
    public class TagServicesTests
    {
        [TestMethod]
        public void GetById_Should_ReturnCorrectUser_When_ParametersAreValid()
        {
            //Arrange

            Tag expectedTag = new Tag()
            {
                Id = 1,
                Name = "Bmw"
            };

            var tagRepositoryMock = new Mock<ITagRepository>();
         
            tagRepositoryMock
                .Setup(repo => repo.GetById(1))
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

            Tag expectedTag = new Tag()
            {
                Id = 1,
                Name = "Bmw"
            };

            var tagRepositoryMock = new Mock<ITagRepository>();

            tagRepositoryMock
                .Setup(repo => repo.GetByName("Bmw"))
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

            List<Tag> tags = new List<Tag>()
            {
                new Tag()
                {
                    Id= 1,
                    Name = "Bmw"
                },

                new Tag()
                {
                    Id= 2,
                    Name = "Fiat",
                },

                new Tag()
                {
                    Id= 3,
                    Name = "Toyota",
                }
            };

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

        // => The first test verifies that an existing tag is correctly returned when all parameters are valid.
        // => The second test verifies the creation of a new tag when the tag name doesn't exist.

        [TestMethod]
        public void CreateTag_Should_ReturnCorrectTag_When_ParametersAreValid()
        {
            //Arrange

            Tag testTag = new Tag()
            {
                Id = 1,
                Name = "TestTag"
            };

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

            var tagRepositoryMock = new Mock<ITagRepository>();

            tagRepositoryMock
                .Setup(repo => repo.GetByName(testTag.Name))
                .Returns(testTag);

            tagRepositoryMock
                .Setup(repo => repo.Create(testTag))
                .Returns(testTag);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act

            Tag actualTag = sut.Create(testTag.Name, loggedUser);

            //Assert

            Assert.AreEqual(testTag, actualTag);
        }

        [TestMethod]
        public void CreateTag_Should_ReturnAnotherCorrectTag_When_ParametersAreValid()
        {
            //Arrange

            Tag testTag = new Tag()
            {
                Id = 1,
                Name = "TestTag"
            };

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

            var tagRepositoryMock = new Mock<ITagRepository>();


            tagRepositoryMock
                .Setup(repo => repo.GetByName(testTag.Name))
                .Throws(new EntityNotFoundException("Tag name doesn't exist."));

            tagRepositoryMock
                .Setup(repo => repo.Create(testTag))
                .Callback<Tag>(tag => testTag = tag);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act

            Tag actualTag = sut.Create(testTag.Name, loggedUser);

            //Assert

            Assert.AreEqual(testTag.Name, actualTag.Name);
        }

        //[TestMethod]
        //public void CreateTag_Should_ThrowException_When_LoggedUserIsNull()
        //{

        //}
        [TestMethod]
        public void DeleteTag_Should__When_UserIsAdmin()
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

            Tag tagToDelete = new Tag()
            {
                Id = 1,
                Name = "Bmw"
            };
               
            var tagRepositoryMock = new Mock<ITagRepository>();
           
            tagRepositoryMock
                .Setup(repo => repo.GetById(tagToDelete.Id))
                .Returns(tagToDelete);
           
            tagRepositoryMock
                .Setup(repo => repo.Delete(tagToDelete.Id))
                .Returns(tagToDelete);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act 

            sut.Delete(1, loggedUser);

            //Assert

            tagRepositoryMock
                .Verify(repo => repo.Delete(tagToDelete.Id), Times.Once);
        }

        [TestMethod]
        public void DeleteTag_Should_ThrowException_When_UserIsNotAdmin()
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

            Tag tagToDelete = new Tag()
            {
                Id = 1,
                Name = "Bmw"
            };

            var tagRepositoryMock = new Mock<ITagRepository>();

            tagRepositoryMock
                .Setup(repo => repo.GetById(tagToDelete.Id))
                .Returns(tagToDelete);

            tagRepositoryMock
                .Setup(repo => repo.Delete(tagToDelete.Id))
                .Returns(tagToDelete);

            var sut = new TagService(tagRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException <UnauthorizedOperationException>(() => sut.Delete(1, loggedUser));
        }

        [TestMethod]
        public void EditTag_Should_When_ParametersAreValid()
        {
            //Arrange

            Tag existingTag = new Tag()
            {
                Id = 1,
                Name = "Bmw"
            };

            Tag editedTag = new Tag()
            {
                Id = 1,
                Name = "NewName"
            };

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
