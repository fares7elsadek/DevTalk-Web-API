using FluentValidation;

namespace DevTalk.Application.Posts.Commands.CreatePosts;

public class CreatePostCommandVlidator:AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandVlidator()
    {
        RuleFor(d => d.Title)
            .NotEmpty().Length(5,300)
            .WithMessage("The maximum length of the post title is 300");

        RuleFor(d => d.Body)
            .NotEmpty().Length(5, 50000)
            .WithMessage("The maximum length of the post body is 50000");

        RuleFor(d => d.Files)
            .Cascade(CascadeMode.Stop) 
            .Must(files => files == null || files.Count < 5)
            .WithMessage("The number of files must be less than 5.")
            .When(d => d.Files != null && d.Files.Count > 0); 
    }
}
