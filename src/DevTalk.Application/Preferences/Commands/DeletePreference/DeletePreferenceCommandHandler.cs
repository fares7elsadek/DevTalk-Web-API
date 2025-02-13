using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Preferences.Commands.DeletePreference;

public class DeletePreferenceCommandHandler(IUserContext userContext,
    IUnitOfWork unitOfWork) : IRequestHandler<DeletePreferenceCommand>
{
    public async Task Handle(DeletePreferenceCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        if (user is null)
            throw new CustomeException("User not authorized");

        var category = await unitOfWork.Category
            .GetOrDefalutAsync(x => x.CategoryId == request.CategoryId);


        if (category is null)
            throw new NotFoundException(nameof(category), request.CategoryId);

        var appUser = await unitOfWork.User.GetOrDefalutAsync(x => x.Id == user.userId,
            IncludeProperties: "Preferences");

        if (appUser is null)
            throw new CustomeException("Something wrong has happened");

        var prefernce = appUser.Preferences.FirstOrDefault(x =>  x.CategoryId == request.CategoryId); 
        if (prefernce is null)
            throw new CustomeException("Prefernce already deleted");

        unitOfWork.Preference.Remove(prefernce);
        await unitOfWork.SaveAsync();
    }
}
