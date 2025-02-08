using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Category.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext,
    IPublisher publisher) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();

        if (!user.IsInRole(UserRoles.Admin))
            throw new CustomeException("User not authorized");

        var category = await unitOfWork.Category.GetOrDefalutAsync(x => x.CategoryId
        == request.CategoryId);

        if (category is null)
            throw new NotFoundException(nameof(category),request.CategoryId);

        unitOfWork.Category.Remove(category);
        await unitOfWork.SaveAsync();
        await publisher.Publish(new DeleteCategoryEvent(request.CategoryId));
    }
}
