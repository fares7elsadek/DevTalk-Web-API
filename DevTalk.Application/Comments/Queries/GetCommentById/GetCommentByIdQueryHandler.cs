using AutoMapper;
using DevTalk.Application.Comments.Dtos;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Comments.Queries.GetCommentById;

public class GetCommentByIdQueryHandler(IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetCommentByIdQuery, CommentDto>
{
    public async Task<CommentDto> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.PostId == null)
            throw new ArgumentNullException("id");
        var Post = await unitOfWork.Post.GetOrDefalutAsync(p => p.PostId == request.PostId,
            IncludeProperties: "Comments");
        if (Post == null)
            throw new NotFoundException(nameof(Post), request.PostId);
        var comment = Post.Comments.SingleOrDefault(c => c.CommentId == request.CommentId);
        if(comment == null)
            throw new NotFoundException(nameof(Comment), request.CommentId);
        var commentDto = mapper.Map<CommentDto>(comment);
        return commentDto;
    }
}
