using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Preferences.Commands.AddManyPreferences;

public class AddManyPreferencesCommandHandler(IUserContext userContext,
    IUnitOfWork unitOfWork) : IRequestHandler<AddManyPreferencesCommand>
{
    public async Task Handle(AddManyPreferencesCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        if (user is null)
            throw new CustomeException("User not authorized");

        var categories = await unitOfWork.Category
            .GetAllWithConditionAsync(x => request.CategoryIds.Contains(x.CategoryId));

        if (categories.ToList().Count != request.CategoryIds.Count())
            throw new CustomeException("Something wrong has happened");

        var appUser = await unitOfWork.User.GetOrDefalutAsync(x => x.Id == user.userId,
            IncludeProperties: "Preferences");

        if (appUser is null)
            throw new CustomeException("Something wrong has happened");

        if (appUser.Preferences.Any(x => request.CategoryIds.Contains(x.CategoryId)))
            throw new CustomeException("Many prefernces are already added");


        var prefernces = new List<Preference>();
        foreach (var category in categories)
        {
            prefernces.Add(new Preference { CategoryId = category.CategoryId , UserId = user.userId });
        }

        await unitOfWork.Preference.AddRangeAsync(prefernces);
        await unitOfWork.SaveAsync();
    }
}
