using MediatR;

namespace DevTalk.Application.Category.Commands.CreateCategory;

public class CreateCategoryEvent:INotification
{
    public string CategoryName { get; set; } = default!;
}
