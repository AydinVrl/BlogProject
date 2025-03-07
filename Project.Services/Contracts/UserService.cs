using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Project.Entities.DTOs;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public class UserService
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly SignInManager<BlogUser> _signInManager;

        public UserService(UserManager<BlogUser> userManager, SignInManager<BlogUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Kullanıcı bilgilerini güncelleme
        public async Task<bool> UpdateUserProfileAsync(string userId, string firstName, string lastName, IFormFile? profileImage)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            user.FirstName = firstName;
            user.LastName = lastName;

            // Profil resmini güncellemek
            if (profileImage != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImage.FileName);

                // Save the image in the wwwroot/images directory
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);
                using (var fileStream = new FileStream(filePath,FileMode.Create))
                {
                    await profileImage.CopyToAsync(fileStream);
                    //user.PictureUrl = Convert.ToBase64String(memoryStream.ToArray()); // Resmi base64 olarak kaydedebilirsiniz.
                }
                user.PictureUrl = "/images/" + uniqueFileName;
            }
            else
            {
                // If no new image is uploaded, keep the existing picture URL or use a default one
                if (string.IsNullOrEmpty(user.PictureUrl))
                {
                    user.PictureUrl = "/images/profileIcon.jpg"; // Default image if no profile picture exists
                }
            }
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        // Kullanıcı şifresini değiştirme
        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result;
        }

        // Kullanıcı profilini alma
        public async Task<BlogUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        // Kullanıcıyı silme
        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var result = await _userManager.DeleteAsync(user);
            return result;
        }

        public async Task<List<BlogDTO>> GetBlogsWithFavoriteStatusAsync(string userId, IEnumerable<BlogDTO> blogs)
        {
            var user = await _userManager.FindByIdAsync(userId); // Get the user by ID

            if (user == null)
            {
                return new List<BlogDTO>(); // If the user is not found, return an empty list
            }

            // Get the user's favorite blog IDs
            var favoriteBlogIds = user.FavoriteBlogs.Select(f => f.BlogId).ToList();

            // Add IsFavorite property to each blog based on the user's favorites
            var blogListWithFavoriteStatus = blogs.Select(blog => new BlogDTO
            {
                Id = blog.Id,
                Title = blog.Title,
                CategoryNames = blog.CategoryNames,
                Description = blog.Description,
                UserName = blog.UserName,
                isFavorite = favoriteBlogIds.Contains(blog.Id) // Check if the blog is in the user's favorites
            }).ToList();

            return blogListWithFavoriteStatus;
        }


    }
}
