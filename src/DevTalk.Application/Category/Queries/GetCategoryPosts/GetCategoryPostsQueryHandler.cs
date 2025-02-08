using AutoMapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Category.Queries.GetCategoryPosts;

public class GetCategoryPostsQueryHandler(IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetCategoryPostsQuery,
    IEnumerable<PostDto>>
{
    public async Task<IEnumerable<PostDto>> Handle(GetCategoryPostsQuery request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.Category.GetOrDefalutAsync(
            x => x.CategoryId == request.CategoryId, IncludeProperties: "Posts");
        
        if(category is null)
            throw new NotFoundException(nameof(category),request.CategoryId);

        var posts = category.Posts.ToList();
        if (!posts.Any())
            throw new Exception("there's no posts avalible for this category");
        var postsDto = mapper.Map<IEnumerable<PostDto>>(posts);
        return postsDto;
    }
}
