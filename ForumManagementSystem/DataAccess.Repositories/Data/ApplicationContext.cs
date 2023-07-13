using DataAccess.Models;
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
        public DbSet<LikePost> LikePosts { get; set; }


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
                     Password = "MTIz",
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
                     Password = "MTIz",
                     IsAdmin = false,
                     IsBlocked = true
                },

				new User
				{
					 Id = 4,
					 FirstName = "Andrei",
					 LastName = "Sokolov",
					 Email = "a.sokolov@gmail.com",
					 Username = "sokolov",
					 Password = "MTIz",
					 IsAdmin = true,
					 IsBlocked = false
				},
				new User
				{
					 Id = 5,
					 FirstName = "Margarita",
					 LastName = "Ivanova",
					 Email = "marg89@gmail.com",
					 Username = "margIvanova",
					 Password = "MTIz",
					 IsAdmin = false,
					 IsBlocked = false
				},
				new User
				{
					 Id = 6,
					 FirstName = "Dimitar",
					 LastName = "Peev",
					 Email = "dim@gmail.com",
					 Username = "dimitarDimitrov",
					 Password = "MTIz",
					 IsAdmin = false,
					 IsBlocked = false
				},
                new User
                {
                     Id = 7,
                     FirstName = "Ivan",
                     LastName = "Apostolov",
                     Email = "Apostolche@gmail.com",
                     Username = "IApostolov99",
                     Password = "MTIz",
                     IsAdmin = true,
                     IsBlocked = false
                },
                 new User
                {
                     Id = 8,
                     FirstName = "Ivan",
                     LastName = "Atanasov",
                     Email = "AtansovGerey@gmail.com",
                     Username = "DreamerTillX",
                     Password = "MTIz",
                     IsAdmin = false,
                     IsBlocked = false
                },
            };

            modelBuilder.Entity<User>().HasData(users);

            //Seed categories

            List<Category> categories = new List<Category>()
            {
                new Category
                {
                    Id = 1,
                    Name = "Asia",
                    Description = "Discussions about all the countries that fall in the Asian continent including the middle eastern countries.",
                    DateTime = DateTime.Now,
                    CountPosts =1,
                    CountComments = 1
                },

                new Category
                {
                    Id = 2,
                    Name = "Europe",
                    Description = "European countries related discussions in this forum and that includes the UK as well you dumbo!",
                    DateTime = DateTime.Now,
                    CountPosts =1,
                    CountComments = 1
                },

                new Category
                {
                    Id = 3,
                    Name = "North America",
                    Description = "Yes USA and Canada and whatever else is up there. Please feel free to ask why they drive on the wrong side of the road if you like.",
                    DateTime = DateTime.Now,
                    CountPosts =1,
                    CountComments = 0
                },

                new Category
                {
                    Id = 4,
                    Name = "Others",
                    Description = "Discussions about Antarctica or anything else.",
                    DateTime = DateTime.Now,
                    CountPosts =3,
                    CountComments = 0
                },
				new Category
				{
					Id = 5,
					Name = "Islands",
					Description = "Islands, an enchanting realm surrounded by vast bodies of water. Captures the imagination of adventurers.",
					DateTime = DateTime.Now,
					CountPosts =3,
					CountComments = 2
				},
				new Category
				{
					Id = 6,
					Name = "Children's World",
					Description = "Delightful journey to discover exciting places around the globe that offer endless fun and unforgettable experiences!",
					DateTime = DateTime.Now,
					CountPosts =3,
					CountComments = 3
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
                  DateTime = DateTime.Now,
                  PostCommentsCount = 1
                  },

                new Post
                  {
                  Id=2,
                  Title = "Things To Do In Windsor",
                  Content = "So the help which I require is that I would like to know what things to do in Windsor?",
                  UserId = 3,
                  CategoryId = 2,
                  DateTime = DateTime.Now,
                  PostCommentsCount = 1
                  },

                new Post
                  {
                  Id=3,
                  Title = "Camping In The Northwest",
                  Content = "Any recommendations of areas to look into in either Washington or Northern California?",
                  UserId = 3,
                  CategoryId = 3,
                  DateTime = DateTime.Now
                  },

				new Post
				  {
				  Id=4,
				  Title = "Secret places, where not many people have heard of?",
				  Content = "Are you willing to share if you have visited a place like this?",
				  UserId = 2,
				  CategoryId = 4,
				  DateTime = DateTime.Now
				  },

				new Post
				  {
				  Id=5,
				  Title = "Exploring Coastal Gems: California's Central Coast",
				  Content = "If you're seeking a coastal adventure filled with breathtaking landscapes and hidden treasures, California's Central Coast is the perfect destination to explore",
				  UserId = 3,
				  CategoryId = 4,
				  DateTime = DateTime.Now
				  },

				new Post
				  {
				  Id=6,
				  Title = "Interesting caves all around the world",
				  Content = "Explore some of the most fascinating caves found across the globe. Please share your experience!",
				  UserId = 1,
				  CategoryId = 4,
				  DateTime = DateTime.Now
				  },

				new Post
				  {
				  Id=7,
				  Title = "Tropical Paradise: Exquisite Maldives",
				  Content = "Discover a tropical paradise like no other, where white-sand beaches meet crystal-clear lagoons and vibrant coral reefs.",
				  UserId = 3,
				  CategoryId = 5,
				  DateTime = DateTime.Now,
				  PostCommentsCount = 1
				  },

				new Post
				  {
				  Id=8,
				  Title = "Galapagos Islands' Unique Biodiversity",
				  Content = "Delve into a world where Charles Darwin's theory of evolution came to life, as these islands boast an extraordinary array of wildlife found nowhere else on Earth.",
				  UserId = 4,
				  CategoryId = 5,
				  DateTime = DateTime.Now,
				  PostCommentsCount = 1
				  },

				new Post
				  {
				  Id=9,
				  Title = "Rich Heritage of Japan's Seto Inland Sea Islands",
				  Content = "A collection of over 3,000 islands steeped in history and tradition!",
				  UserId = 2,
				  CategoryId = 5,
				  DateTime = DateTime.Now
				  },

				new Post
				  {
				  Id=10,
				  Title = "Exploring the Enchanting World of Fairy Tales",
				  Content = "Discover beloved stories like Cinderella, Snow White, and Little Red Riding Hood, and explore the lessons of bravery, kindness, and perseverance they teach us.",
				  UserId = 6,
				  CategoryId = 6,
				  DateTime = DateTime.Now,
				  PostCommentsCount = 1
				  },

				new Post
				  {
				  Id=11,
				  Title = "Animal Kingdom: Marvels of Wildlife and Nature",
				  Content = "Diverse habitats that animals call home, from the African savannah to the depths of the Amazon rainforest!",
				  UserId = 6,
				  CategoryId = 6,
				  DateTime = DateTime.Now,
				  PostCommentsCount = 2
				  },

				new Post
				  {
				  Id=12,
				  Title = "Science Explorers: Unleashing Curiosity",
				  Content = "Found places like this? Where experiments explore physics, chemistry, and biology, sparking curiosity and nurturing a love for scientific inquiry!",
				  UserId = 5,
				  CategoryId = 6,
				  DateTime = DateTime.Now,
				  PostCommentsCount = 1
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
                },

				 new Comment()
				{
					Id= 3,
					UserId=3,
					PostId = 7,
					Content = "I can't wait to see your suggestions!",
					DateTime = DateTime.Now
				},

				  new Comment()
				{
					Id= 4,
					UserId=4,
					PostId = 8,
					Content = "Wow, nice place you have explored!",
					DateTime = DateTime.Now
				},

				   new Comment()
				{
					Id= 5,
					UserId=6,
					PostId = 10,
					Content = "Hm, this should be interesting..or not?",
					DateTime = DateTime.Now
				},

					new Comment()
				{
					Id= 6,
					UserId=6,
					PostId = 11,
					Content = "I am just here for the show.. I will grab my popcorns!",
					DateTime = DateTime.Now
				},

					 new Comment()
				{
					Id= 7,
					UserId=5,
					PostId = 12,
					Content = "Where to start? It will be a long one..",
					DateTime = DateTime.Now
				},

					  new Comment()
				{
					Id= 8,
					UserId=5,
					PostId = 11,
					Content = "Ok, we are waiting.. Come on, you!",
					DateTime = DateTime.Now
				}
			};

            modelBuilder.Entity<Comment>().HasData(comments);

            modelBuilder.Entity<Comment>()
                       .HasOne(c => c.CreatedBy)
                       .WithMany(u => u.Comments)
                       .HasForeignKey(c => c.UserId)
                       .OnDelete(DeleteBehavior.NoAction);


            //Seed tags

            List<Tag> tags = new List<Tag>()
            {
                new Tag()
                {
                    Id= 1,
                    Name = "Windsor"
                },

                new Tag()
                {
                    Id= 2,
                    Name = "kitchen",
                },

                new Tag()
                {
                    Id= 3,
                    Name = "Camping",
                }
            };

            modelBuilder.Entity<Tag>().HasData(tags);

            //Seed postTags

            List<PostTag> postTags = new List<PostTag>()
            {
                new PostTag()
                {
                    Id= 1,
                    PostId=2,
                    TagId=1,
                },

                new PostTag()
                {
                    Id= 2,
                    PostId=3,
                    TagId=3,
                },

                new PostTag()
                {
                    Id= 3,
                    PostId=2,
                    TagId=1,
                },

                new PostTag()
                {
                    Id= 4,
                    PostId=1,
                    TagId=3,
                }
            };

            modelBuilder.Entity<PostTag>().HasData(postTags);


        }

    }
}
