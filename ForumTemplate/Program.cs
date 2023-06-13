
using ForumTemplate.Repositories;
using ForumTemplate.Services;
using ForumTemplate.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ForumTemplate.Validation;

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

            //Validators
            builder.Services.AddScoped<ICommentsValidator, CommentsValidator>();

            // Helpers
            builder.Services.AddScoped<UserMapper>();

            var app = builder.Build();

            app.UseRouting();

            app.MapControllers();

            app.Run();

        }
    }
}
