using Castle.Core.Resource;
using Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Context
{
    public class AppDbContext : IdentityDbContext<BlogUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FavBlog> FavBlogs { get; set; }
        public DbSet<FavAuthor> FavAuthors { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<Blog>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

       

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
               .HasOne(c => c.Blog)
               .WithMany(b => b.Comments)
               .HasForeignKey(c => c.BlogId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FavBlog>()
            .HasKey(f => new { f.UserId, f.BlogId });  // Composite primary key

            builder.Entity<FavBlog>()
                .HasOne(f => f.User)
                .WithMany(u => u.FavoriteBlogs)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Kullanıcı silinirse, blog da silinsin

            builder.Entity<FavBlog>()
                .HasOne(f => f.Blog)
                .WithMany()
                .HasForeignKey(f => f.BlogId)
                .OnDelete(DeleteBehavior.Cascade);  // Blog silinirse, favori de silinsin

            // FavAuthor için composite key tanımlaması
            builder.Entity<FavAuthor>()
                .HasKey(fa => new { fa.UserId, fa.AuthorId });

            builder.Entity<FavAuthor>()
                .HasOne(fa => fa.User)
                .WithMany(u => u.FavoriteAuthors)
                .HasForeignKey(fa => fa.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // Kullanıcı silinirse, yazar silinmesin

            builder.Entity<FavAuthor>()
                .HasOne(fa => fa.Author)
                .WithMany()
                .HasForeignKey(fa => fa.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);  // Yazar silinirse, favoriler silinmesin


            builder.Entity<BlogCategory>()
        .HasKey(bc => new { bc.BlogId, bc.CategoryId }); // Composite Key

            builder.Entity<BlogCategory>()
                .HasOne(bc => bc.Blog)
                .WithMany(b => b.BlogCategories)
                .HasForeignKey(bc => bc.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BlogCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BlogCategories)
                .HasForeignKey(bc => bc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Blogger",
                    NormalizedName = "BLOGGER"
                },
                                new IdentityRole
                                {
                                    Id = "3",
                                    Name = "Editor",
                                    NormalizedName = "EDITOR"
                                });

            var hasher = new PasswordHasher<BlogUser>();

            builder.Entity<BlogUser>().HasData(
                new BlogUser
                {
                    Id = "1",
                    UserName = "admin@mail.com",
                    NormalizedUserName = "ADMIN@MAIL.COM",
                    Email = "admin@mail.com",
                    NormalizedEmail = "ADMIN@MAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin+123")
                });

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "1",
                    RoleId = "1",
                });


        }
    }
}
