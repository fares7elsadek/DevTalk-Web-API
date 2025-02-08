using MediatR;

namespace DevTalk.Application.Category.Commands.DeleteCategory;

public class DeleteCategoryEvent(string categoryId) : INotification
{
    public string CategoryId { get; set; } = categoryId;
}
