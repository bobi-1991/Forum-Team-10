
using ForumTemplate.Repositories;
using ForumTemplate.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ForumTemplate.Validation;
using ForumTemplate.Services.Authentication;
using ForumTemplate.Repositories.UserPersistence;
using ForumTemplate.Repositories.CommentPersistence;
using ForumTemplate.Repositories.PostPersistence;
using ForumTemplate.Services.CommentService;
using ForumTemplate.Services.PostService;
using ForumTemplate.Services.UserService;

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


            //Validators
            builder.Services.AddScoped<ICommentsValidator, CommentsValidator>();
            builder.Services.AddScoped<IPostsValidator, PostsValidator>();

            // Helpers
            builder.Services.AddScoped<UserMapper>();
            builder.Services.AddScoped<PostMapper>();

            var app = builder.Build();

            app.UseRouting();

            app.MapControllers();

            app.Run();

        }
    }
}
