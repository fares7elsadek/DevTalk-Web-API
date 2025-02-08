using AutoMapper;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.Category.Dtos;

public class CategoryProfile:Profile
{
    public CategoryProfile()
    {
        CreateMap<Categories, CategoryDto>()
            .ReverseMap();
    }
}
