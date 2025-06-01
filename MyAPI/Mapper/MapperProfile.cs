using AutoMapper;
using BusinessObjects;
using FUDTOs;

namespace MyAPI.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.ParentCategoryName, opt =>
                    opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.CategoryName : "None"));
            CreateMap<NewsArticle, NewsArticleDTO>().ReverseMap();
            CreateMap<SystemAccount, SystemAccountDTO>().ReverseMap();
            CreateMap<Tag, TagDTO>().ReverseMap();
        }
    }
}
