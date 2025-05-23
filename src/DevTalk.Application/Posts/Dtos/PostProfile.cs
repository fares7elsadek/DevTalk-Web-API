﻿using AutoMapper;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using Humanizer;


namespace DevTalk.Application.Posts.Dtos;

public class PostProfile:Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostDto>()
            .ForMember(d => d.User,opt =>
            {
                opt.MapFrom(src => src.User);
            })
            .ForMember(d => d.PostMedias,opt =>
            {
                opt.MapFrom(src => src.PostMedias);

            }).ForMember(d => d.Votes, opt =>
            {
                opt.MapFrom(src => src.Votes);
            })
            .ForMember(d => d.UpVotes, opt =>
            {
                opt.MapFrom(src => src.Votes.Where(v => v.VoteType == VoteType.UpVote).Count());

            })
            .ForMember(d => d.DownVotes, opt =>
            {
                opt.MapFrom(src => src.Votes.Where(v => v.VoteType == VoteType.DownVote).Count());

            })
            .ForMember(d => d.Comments, opt =>
            {
                opt.MapFrom(src => src.Comments.Count());

            })
            .ForMember(dest => dest.Categories
            , opt => opt.MapFrom(src => src.Categories
            .Select(c => c.CategoryName))).ReverseMap();
    }
}
