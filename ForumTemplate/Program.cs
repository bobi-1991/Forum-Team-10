
using ForumTemplate.Mappers;
using ForumTemplate.Validation;
using ForumTemplate.Services.CommentService;
using ForumTemplate.Services.PostService;
using ForumTemplate.Services.UserService;
using FluentValidation;
using ForumTemplate.Common;
using FluentValidation.AspNetCore;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Persistence.CommentRepository;
using ForumTemplate.Authorization;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using ForumTemplate.Data;

namespace ForumTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // Database
            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();

            // Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ICommentService, CommentService>();

            //Validators
            builder.Services.AddScoped<CommentsValidator>();
            builder.Services.AddScoped<PostsValidator>();
            builder.Services.AddScoped<UserAuthenticationValidator>();

            // Fluent Validation
            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

            // Helpers
            builder.Services.AddScoped<UserMapper>();
            builder.Services.AddScoped<PostMapper>();
            builder.Services.AddScoped<CommentMapper>();
            builder.Services.AddScoped<IAuthManager, AuthManager>();

            var app = builder.Build();

            app.UseRouting();

            app.MapControllers();

            app.Run();

        }
    }
}
