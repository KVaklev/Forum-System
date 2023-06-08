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
            builder.Services.AddSingleton<ICommentRepository, CommentRepository>();


            //Services
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ICommentService, CommentService>();

            //Helpers
            builder.Services.AddScoped<UserMapper>();
            builder.Services.AddScoped<CategoryMapper>();
            builder.Services.AddScoped<PostMapper>();
            builder.Services.AddScoped<CommentMapper>();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.

            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}