using AutoMapper;
using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Bookmark.Dtos;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Bookmark.Queries.GetAllBookmarks;

public class GetAllBookmarksQueryHandler(
    IUnitOfWork unitOfWork,IMapper mapper) : IRequestHandler<GetAllBookmarksQuery, IEnumerable<BookmarkDto>>
{
    public async Task<IEnumerable<BookmarkDto>> Handle(GetAllBookmarksQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        var bookmarks = await unitOfWork.Bookmark.GetAllWithPagination(x => x.UserId == userId,
            request.Page,
            request.Size,IncludeProperties:"Post");

        if (!bookmarks.Any())
            throw new CustomeException("There's no bookmarks yet!");

        var bookmarksDto = mapper.Map<IEnumerable<BookmarkDto>>(bookmarks);
        return bookmarksDto;
    }
}
