using AutoMapper;
using DevTalk.Application.Caching;
using DevTalk.Application.Category.Dtos;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Category.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await unitOfWork.Category.GetAllAsync();
        var categoriesDto = mapper.Map<IEnumerable<CategoryDto>>(categories);
        return categoriesDto;
    }
}
