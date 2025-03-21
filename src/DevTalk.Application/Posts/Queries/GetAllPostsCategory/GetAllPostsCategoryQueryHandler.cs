using AutoMapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevTalk.Application.Posts.Queries.GetAllPostsCategory;

public class GetAllPostsCategoryQueryHandler(IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllPostsCategoryQuery,
    GetUserPostsDto>
{
    public async Task<GetUserPostsDto> Handle(GetAllPostsCategoryQuery request, CancellationToken cancellationToken)
    {
        var decodedTime = DateTimeCursorOperations.Decode(request.timeCursor);
        var posts = await unitOfWork.Post.GetPostsByCategory(request.IdCursor, request.CategoryId, decodedTime
            , request.ScoreCursor, request.PageSize);


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
