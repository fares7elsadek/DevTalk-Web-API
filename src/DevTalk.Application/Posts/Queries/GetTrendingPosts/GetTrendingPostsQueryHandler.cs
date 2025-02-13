﻿using AutoMapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Buffers.Text;
using System.Globalization;
using System.Text;

namespace DevTalk.Application.Posts.Queries.GetTrendingPosts;

public class GetTrendingPostsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetTrendingPostsQuery,
    GetUserPostsDto>
{
    public async Task<GetUserPostsDto> Handle(GetTrendingPostsQuery request, CancellationToken cancellationToken)
    {
        var decodedTime = DateTimeCursorOperations.Decode(request.timeCursor);
        var posts = await unitOfWork.Post.GetTrendingPostsPagination(request.IdCursor, decodedTime
            , request.ScoreCursor, request.PageSize, IncludeProperties: "PostMedias,Votes,Comments,User,Categories");


        var postsDto = mapper.Map<IEnumerable<PostDto>>(posts);

        if (posts.Any())
        {
            var lastPost = posts.LastOrDefault();
            var idcursor = lastPost?.PostId;
            var time = DateTimeCursorOperations.Encode(lastPost!.PostedAt);
            var scoreCursor = lastPost.PopularityScore;



            var resultcursor = new GetUserPostsDto
            {
                Id_cursor = idcursor!,
                time_cursor = time,
                score_cursor = scoreCursor,
                Posts = postsDto
            };
            return resultcursor;
        }

        var result = new GetUserPostsDto
        {
            Id_cursor = "",
            time_cursor = "",
            score_cursor = 0,
            Posts = postsDto
        };

        return result;
    }
}

