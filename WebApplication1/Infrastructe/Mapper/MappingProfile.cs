using AutoMapper;
using Entity.Models; // bu ne ??
using Project.Entities;
using Project.Entities.DTOs;

namespace Project.WebApp.Infrastructe.Mapper
{
    public class MappingProfile  : Profile
    {
        public MappingProfile()
        {
            CreateMap<BlogDTO, Blog>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Blog, CommentDTO>(); // Add this line to map Blog to CommentDTO

            CreateMap<Blog, BlogDTO>()
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
    .ForMember(dest => dest.CategoryNames, opt => opt.MapFrom(src =>
        src.BlogCategories.Select(bc => bc.Category.CategoryName).ToList())); // Correctly mapping to List<string>


        }
    }
}
