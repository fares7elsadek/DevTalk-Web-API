using System.Data;
using AutoMapper;
using Dapper;
using DevTalk.Application.Category.Dtos;
using DevTalk.Domain.Abstractions;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;


namespace DevTalk.Application.Category.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(
    IMapper mapper,ISqlConnectionFactory dapper) : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var sql = @"
                    -- get category by id
                    SELECT * FROM ""Category"" WHERE ""CategoryId"" = @categoryId
                ";
        
        using IDbConnection connection = dapper.CreateConnection();
        var category = await connection.QueryFirstAsync<Categories>(sql, new { categoryId = request.CategoryId });
        if(category is null)
            throw new NotFoundException(nameof(category),request.CategoryId);

        var categoryDto = mapper.Map<CategoryDto>(category);
        return categoryDto;
    }
}
