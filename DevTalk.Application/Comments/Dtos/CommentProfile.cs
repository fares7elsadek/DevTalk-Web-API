using AutoMapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.Comments.Dtos;

public class CommentProfile:Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(d => d.Votes, opt =>
            {
                opt.MapFrom(src => src.Votes);
            })
            .ForMember(d => d.User, opt =>
            {
                opt.MapFrom(src => src.User);

            }).ReverseMap();
    }
}
