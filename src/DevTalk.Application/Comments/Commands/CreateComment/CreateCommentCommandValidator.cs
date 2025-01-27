using FluentValidation;

namespace DevTalk.Application.Comments.Commands.CreateComment;

public class CreateCommentCommandValidator:AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(d => d.CommentText)
            .Length(5, 3000)
            .WithMessage("The comment must be between (5-3000) characters");
    }
}
