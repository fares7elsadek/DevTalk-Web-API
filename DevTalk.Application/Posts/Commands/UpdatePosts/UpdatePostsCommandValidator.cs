using FluentValidation;

namespace DevTalk.Application.Posts.Commands.UpdatePosts;

public class UpdatePostsCommandValidator:AbstractValidator<UpdatePostsCommand>
{
    public UpdatePostsCommandValidator()
    {
        RuleFor(d => d.Title)
            .Length(5, 300)
            .When(d => !string.IsNullOrWhiteSpace(d.Title))
            .WithMessage("The title must be between 5 and 300 characters.");

        RuleFor(d => d.Body)
            .Length(5, 50000)
            .When(d => !string.IsNullOrWhiteSpace(d.Body))
            .WithMessage("The body must be between 5 and 50000 characters.");

        RuleFor(d => d)
            .Must(d => !string.IsNullOrWhiteSpace(d.Title) || !string.IsNullOrWhiteSpace(d.Body))
            .WithMessage("Either the title or the body must be provided.");
    }
}
