
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
using ForumTemplate.Persistence.LikeRepository;
using ForumTemplate.Services.LikeService;
using ForumTemplate.Persistence.TagRepository;
using ForumTemplate.Services.TagService;

namespace ForumTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
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
            builder.Services.AddScoped<ILikeRepository, LikeRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();

            // Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ILikeService, LikeService>();
            builder.Services.AddScoped<ITagService, TagService>();

            //Validators
            builder.Services.AddScoped<ICommentsValidator, CommentsValidator>();
            builder.Services.AddScoped<IPostsValidator, PostsValidator>();
            builder.Services.AddScoped<IUserAuthenticationValidator, UserAuthenticationValidator>();
            builder.Services.AddScoped<ITagsValidator, TagsValidator>();

            // Fluent Validation
            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

            // Helpers
            builder.Services.AddScoped<IUserMapper, UserMapper>();
            builder.Services.AddScoped<IPostMapper, PostMapper>();
            builder.Services.AddScoped<ICommentMapper, CommentMapper>();
            builder.Services.AddScoped<ITagMapper, TagMapper>();
            builder.Services.AddScoped<IAuthManager, AuthManager>();

            //Session
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromSeconds(1000);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

			var app = builder.Build();

		
			app.UseRouting();
            app.UseSession();

			// Enables the views to use resources from wwwroot
			app.UseStaticFiles();

			app.MapDefaultControllerRoute();

            app.Run();

        }
    }
}
