using System.Collections;
using System.Data;
using AutoMapper;
using Dapper;
using DevTalk.Application.Caching;
using DevTalk.Application.Category.Dtos;
using DevTalk.Domain.Abstractions;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Category.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler(
    IMapper mapper,ISqlConnectionFactory dapper) : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
            var sql = @"    -- get all categories
                            SELECT * FROM ""Category""
                        ";
        using IDbConnection connection = dapper.CreateConnection();
        var categories = await connection.QueryAsync<Categories>(sql);
        var categoriesDto = mapper.Map<IEnumerable<CategoryDto>>(categories);
        return categoriesDto;
    }
}
