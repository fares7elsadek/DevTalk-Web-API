using AutoMapper;
using DevTalk.Application.Category.Dtos;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;


namespace DevTalk.Application.Category.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.Category
            .GetOrDefalutAsync(x => x.CategoryId ==  request.CategoryId);
        if(category is null)
            throw new NotFoundException(nameof(category),request.CategoryId);

        var categoryDto = mapper.Map<CategoryDto>(category);
        return categoryDto;
    }
}
