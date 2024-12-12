using AutoMapper;
using DevTalk.Application.Comments.Dtos;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Comments.Queries.GetAllCommentsByPost;

public class GetAllCommentsByPostQueryHandler(IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllCommentsByPostQuery
    , IEnumerable<CommentDto>>
{
    public async Task<IEnumerable<CommentDto>> Handle(GetAllCommentsByPostQuery request, CancellationToken cancellationToken)
    {
        if(request.PostId == null)
            throw new ArgumentNullException("id");
        var Post = await unitOfWork.Post.GetOrDefalutAsync(p => p.PostId == request.PostId,
            IncludeProperties: "Comments");
        if(Post == null)
            throw new NotFoundException(nameof(Post),request.PostId);
        var comments = Post.Comments.ToList();
        var CommentsDto = mapper.Map<IEnumerable<CommentDto>>(comments);
        return CommentsDto;
    }
}
