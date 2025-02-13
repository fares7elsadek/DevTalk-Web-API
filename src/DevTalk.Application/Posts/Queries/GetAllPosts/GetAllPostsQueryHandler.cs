using AutoMapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Application.Services.Caching;
using DevTalk.Domain.Repositories;
using MediatR;
using Serilog;

namespace DevTalk.Application.Posts.Queries.GetAllPosts;

public class GetAllPostsQueryHandler(IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<GetAllPostsQuery,GetAllPostsDto>
{
    public async Task<GetAllPostsDto> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.Cursor, out _))
            request.Cursor = "";

        var Posts = await unitOfWork.Post.GetAllPostsPagination(request.Cursor,request.PageSize,IncludeProperties: "PostMedias,Votes,Comments,User,Categories");
        var PostDto = mapper.Map<IEnumerable<PostDto>>(Posts);

        var lastPost = Posts.LastOrDefault();
        var nextCursorId = lastPost?.PostId;
        var resutlDto = new GetAllPostsDto
        {
            Id_cursor = nextCursorId!,
            Posts = PostDto
        };
        return resutlDto;
    }
}
