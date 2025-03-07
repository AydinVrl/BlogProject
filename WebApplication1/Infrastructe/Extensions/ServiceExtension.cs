using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Project.Repositories.Abstracts;
using Project.Repositories.Contracts;
using Project.Repositories.UnitOfWork;
using Project.Services.Abstracts;
using Project.Services.Contracts;
using Repository.Context;
using System.Runtime.InteropServices;

namespace Project.WebApp.Infrastructe.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConn"), b => b.MigrationsAssembly("Project.WebApp")).UseLazyLoadingProxies();
            });
        }

        public static void ConfigureRepositoryRegister(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
        }
        public static void ConfigureServiceRegister(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommentService, CommentService>();
        }
    }
}
