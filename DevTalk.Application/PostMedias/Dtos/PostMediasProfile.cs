using AutoMapper;
using DevTalk.Domain.Entites;
namespace DevTalk.Application.PostMedias.Dtos;

public class PostMediasProfile:Profile
{
    public PostMediasProfile()
    {
        CreateMap<PostMedia, PostMediasDto>().ReverseMap(); 
    }
}
