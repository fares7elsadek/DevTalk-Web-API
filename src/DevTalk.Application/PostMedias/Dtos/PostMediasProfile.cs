using AutoMapper;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
namespace DevTalk.Application.PostMedias.Dtos;

public class PostMediasProfile:Profile
{
    public PostMediasProfile()
    {
        CreateMap<PostMedia, PostMediasDto>()
         .ForMember(dest => dest.Type, opt =>
        opt.MapFrom(src => src.Type == PostMediaTypes.Image ? "Image" : "Video"))
        .ReverseMap()
        .ForMember(dest => dest.Type, opt =>
        opt.MapFrom(src => src.Type == "Image" ? PostMediaTypes.Image : PostMediaTypes.Video));

    }
}
