using MediatR;

namespace DevTalk.Application.Category.Commands.DeleteCategory;

public class DeleteCategoryCommand(string categoryId):IRequest
{
    public string CategoryId { get; set; } = categoryId;
}
