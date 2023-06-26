using Business.Dto;
using DataAccess.Models;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Tests.Helpers
{
    public class TestHelpers
    {
        //Helpers for CategoryService Tests

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
        public static Category GetTestCategoryToUpdate()
        {
            return new Category
            {
                Id = 1,
                Name = "Asia",
                Description = "Discussions about all the countries.",
                DateTime = DateTime.Now
            };
        }
        public static Category GetTestCategoryWithDuplicateName()
        {
            return new Category
            {
                Id = 8,
                Name = "Asia",
                Description = "Discussions about all the countries.",
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
                PhoneNumber = "0897554285",
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

        //Helpers for UserService Tests

        public static User GetTestCreateUser()
        {
            return new User
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
        }
        public static CreateUserDto GetTestCreateUserDto()
        {
            return new CreateUserDto
            {
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "jdjdnjednjd"
            };
        }
        public static UpdateUserDto GetTestUpdateUserDto()
        {
            return new UpdateUserDto
            {
                Email = "test@gmail.com"
            };
        }
        public static User GetTestDeleteUser()
        {
            return new User
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
        }
        public static User GetTestUpdateUser()
        {
            return new User
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
        }
        public static User GetTestExpectedUserAsAdmin()
        {
            return new User
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = true,
                IsBlocked = false
            };
        }
        public static User GetTestExpectedUserAsBlocked()
        {
            return new User
            {
                Id = 2,
                FirstName = "Mariq",
                LastName = "Petrova",
                Email = "m.petrova@gmail.com",
                Username = "mariicheto",
                Password = "wdljsl",
                PhoneNumber = "0897554285",
                IsAdmin = false,
                IsBlocked = true
            };
        }
        public static User GetTestExpectedUserAsUnblocked()
        {
            return new User
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
        }
        public static User GetTestUpdateUserInfo()
        {
            return new User
            {
                Id = 2,
                FirstName = "Mareto",
                LastName = "Petrovka",
                Username = null
            };
        }
        public static List<User> GetTestListUsers()
        {
            return new List<User>()
            {
                new User
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
                },

                new User
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
                },

                new User
                {
                    Id = 3,
                    FirstName = "Mara",
                    LastName = "Dobreva",
                    Email = "m.dobreva@gmail.com",
                    Username = "marcheto",
                    Password = "fjsdda",
                    PhoneNumber = "0797556285",
                    IsAdmin = false,
                    IsBlocked = false
                }

            };
        }
        public static List<User> GetTestExpectedListUsers()
        {
            return new List<User>()
            {
                new User
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
                }
            };
        }
        public static List<GetUserDto> GetTestExpectedListDtoUsers()
        {
            return new List<GetUserDto>()
            {
                new GetUserDto
                {
                    FirstName = "Ivan",
                    LastName = "Draganov",
                    Email = "i.draganov@gmail.com",
                    Username = "ivanchoDraganchov",
                }
            };
        }

            //Helpers for TagService Tests

        public static Tag GetTestTag()
        {
            return new Tag
            {
                Id = 1,
                Name = "Bmw"
            };
        }
        public static Tag GetTestEditTag()
        {
            return new Tag
            {
                Id = 1,
                Name = "NewName"
            };
        }
        public static List<Tag> GetTestListTags()
        {
            return new List<Tag>()
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
        }

        //Helpers for CommentService Tests

        public static Comment GetTestComment()
        {
            return new Comment()
            {
                Id = 1,
                UserId = 1,
                PostId = 1,
                Content = "The best town!",
                DateTime = DateTime.Now
            };
        }
        public static Comment GetTestUpdateComment()
        {
            return new Comment
            {
                Id = 1,
                UserId = 1,
                Content = "The biggest town!"
            };
        }
        public static List<Comment> GetTestListComments()
        {
            return new List<Comment>()
            {
                new Comment()
                {
                    Id= 1,
                    UserId=1,
                    PostId = 1,
                    Content = "The best town!",
                    DateTime = DateTime.Now
                },

                new Comment()
                {
                    Id= 2,
                    UserId=2,
                    PostId = 2,
                    Content = "The worst town!",
                    DateTime = DateTime.Now
                }
            };
        }
        public static List<Comment> GetTestExpectedListComments()
        {
            return new List<Comment>()
            {
                new Comment()
                {
                    Id= 1,
                    UserId=1,
                    PostId = 1,
                    Content = "The best town!",
                    DateTime = DateTime.Now
                },

                new Comment()
                {
                    Id= 2,
                    UserId=2,
                    PostId = 2,
                    Content = "The worst town!",
                    DateTime = DateTime.Now
                }
            };
        }

        //Helpers for PostService Tests

        public static Post GetTestPost()
        {
            return new Post
            {
                Id = 1,
                Title = "Cooking Your Food",
                Content = "When you are able to get an accommodation that has a kitchen and cooking implements, do you cook your own food? My sister has that style. She cooks breakfast at least so they can save a little money. We once had booked in a small hotel in Hong Kong but we forgo with the cooking. For us, a vacation should be savored to the fullest.",
                UserId = 2,
                CategoryId = 1,
                DateTime = DateTime.Now
            };
        }
           
        public static Post GetTestPostAuthorizedUser()
        {
            return new Post
            {
                Id = 1,
                Title = "Cooking Your Food",
                Content = "When you are able to get an accommodation that has a kitchen and cooking implements, do you cook your own food? My sister has that style. She cooks breakfast at least so they can save a little money. We once had booked in a small hotel in Hong Kong but we forgo with the cooking. For us, a vacation should be savored to the fullest.",
                UserId = 2,
                CategoryId = 1,
                DateTime = DateTime.Now,

            };
        }

        public static LikePost GetLikePostIsLiked()
        {
            return new LikePost()
            {
                Id = 1,
                UserId = 1,
                PostId = 1,
                IsLikedPost = true

            };
        }

        public static LikePost GetLikePostIsNotLiked()
        {
            return new LikePost()
            {
                Id = 1,
                UserId = 1,
                PostId = 1,
                IsLikedPost = false

            };
        }

        public static List<Post> GetTestListPost()
        {
            return new List<Post>()
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
        }
        public static List<string> GetTestListTag()
        {
            return new List<string>() { "tag1", "tag2", "tag3" };
        }
        public static LikeComment GetLikeCommentIsLiked()
        {
            return new LikeComment() 
            {
            Id=1,
            UserId=1,
            CommentId=1,
            IsLiked=true
            };

        }
        public static LikeComment GetLikeCommentIsNoLiked()
        {
            return new LikeComment()
            {
                Id = 1,
                UserId = 1,
                CommentId = 1,
                IsLiked = false
            };

        }
    }
}