using AutoMapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Serilog;

namespace DevTalk.Application.Posts.Queries.GetPostById;

internal class GetPostByIdQueryHandler(IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<GetPostByIdQuery, PostDto>
{
    public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        Log.Information($"Get Post with id {request.PostId}");
        var Post = await unitOfWork.Post.GetOrDefalutAsync(x => x.PostId == request.PostId,
            IncludeProperties: "PostMedias,Votes,Comments,User,Categories");
        if (Post == null) throw new NotFoundException(nameof(Post), request.PostId);
        var PostDto = mapper.Map<PostDto>(Post);
        return PostDto;
    }
}
