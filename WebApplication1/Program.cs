using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Project.WebApp.Infrastructe.Extensions;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Project.Services.Abstracts;
using Project.Services.Contracts;
using Project.Entities.Models;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddApplicationPart(typeof(AssemblyReference).Assembly);

            builder.Services.AddIdentity<BlogUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<UserService>();

            //builder.Services.AddScoped<IBlogService, BlogService>();
            //builder.Services.AddScoped<ICategoryService, CategoryService>();
            //builder.Services.AddScoped<ICommentService, CommentService>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.ConfigureDbContext(builder.Configuration);
            builder.Services.ConfigureRepositoryRegister();
            builder.Services.ConfigureServiceRegister();
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Home/AccessDenied"; // Specify the path to the custom AccessDenied view
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Blog}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
