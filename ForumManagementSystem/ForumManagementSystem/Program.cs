using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Builder;
using Presentation.Helpers;


namespace ForumManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(CustomAutoMapper).Assembly);

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
            builder.Services.AddSingleton<User>();

            //Helpers
           // builder.Services.AddScoped<UserMapper>();
           // builder.Services.AddScoped<PostMapper>();
            builder.Services.AddScoped<CustomAutoMapper>();

            // builder.Services.AddScoped<PostQueryParameters>();

            builder.Services.AddScoped<AuthManager>();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseDeveloperExceptionPage();
            app.UseRouting();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}