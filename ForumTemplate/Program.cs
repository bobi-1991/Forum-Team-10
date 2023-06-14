
using ForumTemplate.Repositories;
using ForumTemplate.Services;
using ForumTemplate.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ForumTemplate.Validation;
using ForumTemplate.Services.Interfaces;
using ForumTemplate.Services.Authentication;
using ForumTemplate.Repositories.UserNewPersistence;

namespace ForumTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // Repositories
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<IPostRepository, PostRepository>();
            builder.Services.AddSingleton<ICommentRepository, CommentRepository>();

            // Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

            // New Services (just to separate the logic)
            builder.Services.AddSingleton<IUserNewRepository, UserNewRepository>();

            //Validators
            builder.Services.AddScoped<ICommentsValidator, CommentsValidator>();
            builder.Services.AddScoped<IPostsValidator, PostsValidator>();

            // Helpers
            builder.Services.AddScoped<UserMapper>();

            var app = builder.Build();

            app.UseRouting();

            app.MapControllers();

            app.Run();

        }
    }
}
