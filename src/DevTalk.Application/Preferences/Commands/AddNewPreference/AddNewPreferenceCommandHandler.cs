using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevTalk.Application.Preferences.Commands.AddNewPreference;

public class AddNewPreferenceCommandHandler(IUserContext userContext,
    IUnitOfWork unitOfWork) : IRequestHandler<AddNewPreferenceCommand>
{
    public async Task Handle(AddNewPreferenceCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        if (user is null)
            throw new CustomeException("User not authorized");

        var category = await unitOfWork.Category
            .GetOrDefalutAsync(x => x.CategoryId == request.CategoryId);


        if (category is null)
            throw new NotFoundException(nameof(category),request.CategoryId);

        var appUser = await unitOfWork.User.GetOrDefalutAsync(x => x.Id==user.userId,
            IncludeProperties: "Preferences" );

        if (appUser is null)
            throw new CustomeException("Something wrong has happened");

        if (appUser.Preferences.Any(x => x.CategoryId == request.CategoryId))
            throw new CustomeException("Prefernce already added");


        var preference = new Preference
        {
            UserId = user.userId,
            CategoryId = request.CategoryId,
        };

        await unitOfWork.Preference.AddAsync(preference);
        await unitOfWork.SaveAsync();
    }
}
