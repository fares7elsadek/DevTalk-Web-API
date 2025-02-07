using AutoMapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Application.Services.Caching;
using DevTalk.Domain.Repositories;
using MediatR;
using Serilog;

namespace DevTalk.Application.Posts.Queries.GetAllPosts;

public class GetAllPostsQueryHandler(IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<GetAllPostsQuery, IEnumerable<PostDto>>
{
    public async Task<IEnumerable<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        Log.Information("Get all posts");
        var Posts = await unitOfWork.Post.GetAllAsync(IncludeProperties: "PostMedias,Votes,Comments,User");
        var PostDto = mapper.Map<IEnumerable<PostDto>>(Posts);
        return PostDto;
    }
}
