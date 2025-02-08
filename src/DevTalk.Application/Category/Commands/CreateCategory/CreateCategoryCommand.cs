using MediatR;

namespace DevTalk.Application.Category.Commands.CreateCategory;

public class CreateCategoryCommand:IRequest
{
    public string CategoryName { get; set; } = default!;
}
