using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // Repositories
            builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<IPostRepository, PostRepository>();


            //Services
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPostService, PostService>();

            //Helpers
            builder.Services.AddScoped<UserMapper>();
<<<<<<< HEAD
            builder.Services.AddScoped<ForumManagementSystem.Models.CategoryMapper>();
            builder.Services.AddScoped<ForumManagementSystem.Models.PostMapper>();



=======
            builder.Services.AddScoped<CategoryMapper>();
            
>>>>>>> 3f94aa6b746f513105afd7651d2ab5234966a339
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.

            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }

    internal class PostMapper
    {
    }
}