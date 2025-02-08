using FluentValidation;

namespace DevTalk.Application.Category.Commands.CreateCategory;

public class CreateCategoryCommandValidator:AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(d => d.CategoryName)
            .NotEmpty().Length(5, 300)
            .WithMessage("The maximum length of the category name is 300");
    }
}
