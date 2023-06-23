using ForumManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumManagementSystem.Tests.Helpers
{
    public class TestHelpers
    {
        public static Category GetTestCategory()
        {
            return new Category
            {
                Id = 1,
                Name = "Asia",
                Description = "Discussions about all the countries that fall in the Asian continent including the middle eastern countries.",
                DateTime = DateTime.Now
            };
        }

        public static Category GetTestNewCategory()
        {
            return new Category
            {
                Id = 9,
                Name = "Afrika",
                Description = "Discussions about all the countries.",
                DateTime = DateTime.Now
            };
        }

        public static List<Category> GetTestFilterCategory()
        {
            return new List<Category>
            {
                new Category()
                {
                    Id = 1,
                    Name = "Asia",
                    Description = "Discussions about all the countries that fall in the Asian continent including the middle eastern countries.",
                    DateTime = DateTime.Now
                }
            };
        }
        public static User GetTestUser()
        {
            return new User
            {
                Id = 1,
                FirstName = "TestFirst",
                LastName = "TestLast",
                Email = "test@gmail.com",
                Username = "testUsername",
                Password = "MTIz",
                IsAdmin = false,
                IsBlocked = false
            };
        }

        public static User GetTestUserAdmin()
        {
            return new User
            {
                Id = 1,
                FirstName = "TestFirst",
                LastName = "TestLast",
                Email = "test@gmail.com",
                Username = "testUsername",
                Password = "MTIz",
                IsAdmin = true,
                IsBlocked = false
            };
        }
        public static List<Category> GetTestListCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Asian",
                    Description = "Discussions about all the countries that fall in the Asian continent including the middle eastern countries.",
                    DateTime = DateTime.Now
                },

                new Category
                {
                    Id = 2,
                    Name = "Europe",
                    Description = "European countries related discussions in this forum and that includes the UK as well you dumbo!",
                    DateTime = DateTime.Now
                },

                new Category
                {
                    Id = 3,
                    Name = "North America",
                    Description = "Yes USA and Canada and whatever else is up there. Please feel free to ask why they drive on the wrong side of the road if you like.",
                    DateTime = DateTime.Now
                },

                new Category
                {
                    Id = 7,
                    Name = "Others",
                    Description = "Discussions about Antarctica or anything else.",
                    DateTime = DateTime.Now
                },

            };
        }
    }
}
