using FluentValidation;

namespace DevTalk.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommandValidator:AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(d => d.CommentText)
            .Length(5, 3000)
            .WithMessage("The comment must be between (5-3000) characters");
    }
}
