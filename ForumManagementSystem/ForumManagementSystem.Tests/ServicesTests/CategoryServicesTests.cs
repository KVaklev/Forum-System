using Business.Exceptions;
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
    public class CategoryServicesTests
    {
        [TestMethod]
        public void GetById_Should_ReturnCorrectCategory_When_ParametersAreValid()
        {
            //Arrange

            Category expectedCategory = TestHelpers.GetTestCategory();
            

            var categoryRepositoryMock = new Mock<ICategoryRepository>();


            categoryRepositoryMock
                .Setup(repo => repo.GetById(1))
                .Returns(expectedCategory);

            var sut = new CategoryService(categoryRepositoryMock.Object);

            //Act

            Category actualCategory = sut.GetById(expectedCategory.Id);

            //Assert

            Assert.AreEqual(expectedCategory, actualCategory);

        }
        [TestMethod]
        public void GetById_Should_ThrowException_When_CategoryIsNotFound()
        {
            //Arrange

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
           
            categoryRepositoryMock
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Throws(new EntityNotFoundException($"Category with id={It.IsAny<int>()} doesn't exist."));

            var sut = new CategoryService(categoryRepositoryMock.Object);

            //Act & Assert
            Assert.ThrowsException<EntityNotFoundException>(() => sut.GetById(1));

        }
        [TestMethod]
        public void CreateCategory_Should_ReturnCorrectCategory_When_ParametersAreValid()
        {
            //Arrange
            Category testCategory = TestHelpers.GetTestNewCategory();

            User testUser = TestHelpers.GetTestUserAdmin();

            var categoryRepositoryMock = new Mock<ICategoryRepository>();

           categoryRepositoryMock
                .Setup(repo => repo.GetByName(It.IsAny<string>()))
                .Returns(testCategory);

            var sut = new CategoryService(categoryRepositoryMock.Object);

            //Act

            Category actualCategory = sut.Create(testCategory, testUser);

            //Assert

            Assert.AreEqual(testCategory, actualCategory);
        }
        [TestMethod]
        public void CreateCategory_Should_ThrowException_When_ParametersAreNotValid()
        {
            //Arrange

            Category testCategory = TestHelpers.GetTestCategory();
            User testUser = TestHelpers.GetTestUserAdmin();

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
           
            categoryRepositoryMock
                .Setup(repo => repo.GetByName(It.IsAny<string>()))
                .Returns(testCategory);

            var sut = new CategoryService(categoryRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<DuplicateEntityException>(() => sut.Create(testCategory, testUser));
        }
        [TestMethod]
        public void CreateCategory_Should_ThrowException_When_UserIsNotAdmin()
        {
            //Arrange

            Category testCategory = TestHelpers.GetTestCategory();
            User testUser = TestHelpers.GetTestUser();

            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(repo => repo.GetByName(It.IsAny<string>()))
                .Returns(testCategory);

            var sut = new CategoryService(categoryRepositoryMock.Object);

            //Act & Assert

            Assert.ThrowsException<UnauthorizedOperationException> (() => sut.Create(testCategory, testUser));
        }

        [TestMethod]
        public void GetAllCategories_Should_ReturnCorrectList_When_ParametersAreValid()
        {
            //Arrange

            List<Category> categories = TestHelpers.GetTestListCategories();
                
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(repo => repo.GetAll())
                .Returns(categories);

            var sut = new CategoryService(categoryRepositoryMock.Object);

            //Act

            var expectedCategory = sut.GetAll();

            //Assert

            Assert.AreEqual(expectedCategory,categories);

        }

        [TestMethod]
        public void DeleteCategory_Should_When_UserIsAdmin()
        {
            //Arrange 
            Category testCategory = TestHelpers.GetTestCategory();
            User testUser = TestHelpers.GetTestUserAdmin();

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            

            categoryRepositoryMock
                .Setup(repo => repo.GetById(1))
                .Returns(testCategory);
            
            var sut = new CategoryService(categoryRepositoryMock.Object);

            //Act 

            sut.Delete(1,testUser);

            //Assert

            categoryRepositoryMock
                .Verify(repo => repo.Delete(1), Times.Once);
            
        }

        [TestMethod]
        public void DeleteCategory_ThrowException_When_UserIsNotAdmin()
        {
            //Arrange 
            Category testCategory = TestHelpers.GetTestCategory();
            User testUser = TestHelpers.GetTestUser();

            var categoryRepositoryMock = new Mock<ICategoryRepository>();


            categoryRepositoryMock
                .Setup(repo => repo.GetById(1))
                .Returns(testCategory);

            var sut = new CategoryService(categoryRepositoryMock.Object);

            //Act 

            sut.Delete(1, testUser);

            //Assert
                Assert.ThrowsException<UnauthorizedOperationException>(() => sut.Delete(1,testUser));

        }
        [TestMethod]
        public void FilterBy_Should_ReturnCorrectList_When_ParametersAreValid()
        {
            //Arrange
            List<Category> expectedCategory = TestHelpers.GetTestFilterCategory();
            
            CategoryQueryParameter filterParameter = new CategoryQueryParameter()
            {
                Name = "Asia"
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(repo => repo.FilterBy(filterParameter))
                .Returns(expectedCategory);

            var sut = new CategoryService(categoryRepositoryMock.Object);

            // Act

            List<Category> filteredCategory = sut.FilterBy(filterParameter);

            // Assert

            Assert.AreEqual(filteredCategory, expectedCategory);
        }
    }
}
