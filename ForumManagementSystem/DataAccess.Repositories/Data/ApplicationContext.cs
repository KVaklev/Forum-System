﻿using DataAccess.Models;
using ForumManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }

        //Configure DB tables 
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<LikeComment> LikeComments { get; set; }


        //Seed database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seed users

            List<User> users = new List<User>()
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
                     IsAdmin = false,
                     IsBlocked = false
                },

                new User
                {
                     Id=3,
                     FirstName = "Mara",
                     LastName = "Dobreva",
                     Email = "m.dobreva@gmail.com",
                     Username = "marcheto",
                     Password = "fjsdda",
                     IsAdmin = false,
                     IsBlocked = false
                }
            };

            modelBuilder.Entity<User>().HasData(users);

            //Seed categories

            List<Category> categories = new List<Category>()
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

            modelBuilder.Entity<Category>().HasData(categories);

            //Seed posts

            List<Post> posts = new List<Post>()
            {
                new Post
                 {
                  Id=1,
                  Title = "Cooking Your Food",
                  Content = "When you are able to get an accommodation that has a kitchen and cooking implements, do you cook your own food? My sister has that style. She cooks breakfast at least so they can save a little money. We once had booked in a small hotel in Hong Kong but we forgo with the cooking. For us, a vacation should be savored to the fullest.",
                  UserId = 2,
                  CategoryId = 1,
                  DateTime = DateTime.Now
                  },

                new Post
                  {
                  Id=2,
                  Title = "Things To Do In Windsor",
                  Content = "So the help which I require is that I would like to know what things to do in Windsor?",
                  UserId = 3,
                  CategoryId = 2,
                  DateTime = DateTime.Now
                  },

                new Post
                  {
                  Id=3,
                  Title = "Camping In The Northwest",
                  Content = "Any recommendations of areas to look into in either Washington or Northern California?",
                  UserId = 3,
                          CategoryId = 3,
                  DateTime = DateTime.Now
                  }
            };

            modelBuilder.Entity<Post>().HasData(posts);

            //Seed comments

            List<Comment> comments = new List<Comment>()
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

            modelBuilder.Entity<Comment>().HasData(comments);

            //Seed tags

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

            modelBuilder.Entity<Tag>().HasData(tags);
        }

    }
}
