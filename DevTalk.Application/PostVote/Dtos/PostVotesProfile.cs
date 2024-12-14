using AutoMapper;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.PostVote.Dtos;

public class PostVotesProfile:Profile
{
    public PostVotesProfile()
    {
        CreateMap<PostVotes, PostVotesDto>()
            .ForMember(d => d.VoteType,opt =>
            {
                opt.MapFrom(d => d.VoteType == VoteType.UpVote ? "UpVote" : "DownVote");
            })
            .ReverseMap();
    }
}
