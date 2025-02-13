using AutoMapper;
using DevTalk.Application.Comments.Dtos;
using DevTalk.Domain.Entites;
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

        var comments = await unitOfWork.Comment.GetAllWithPagination(p => p.PostId == request.PostId,request.Page,
            request.Size,
            IncludeProperties: "Comments");

        if(comments == null)
            throw new NotFoundException(nameof(Post),request.PostId);

        var CommentsDto = mapper.Map<IEnumerable<CommentDto>>(comments);
        return CommentsDto;
    }
}
