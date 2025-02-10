using AutoMapper;
using DevTalk.Application.Bookmark.Dtos;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Bookmark.Queries.GetBookmarkById;

public class GetBookmarkByIdQueryHandler(IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetBookmarkByIdQuery, BookmarkDto>
{
    public async Task<BookmarkDto> Handle(GetBookmarkByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var user = await unitOfWork.User.GetOrDefalutAsync(x => x.Id == userId,
            IncludeProperties: "Bookmarks");
        if (user is null)
            throw new CustomeException("Something wrong has happened");
        var bookmark = user.Bookmarks.FirstOrDefault(x => x.BookmarkId == request.BookmarkId);
        if (bookmark is null)
            throw new NotFoundException(nameof(bookmark),request.BookmarkId);

        var bookmarkDto = mapper.Map<BookmarkDto>(bookmark);
        return bookmarkDto;
    }
}
