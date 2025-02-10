using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevTalk.Application.Category.Commands.CreateCategory;

public class CreateCategoryCommandHandler(IUnitOfWork unitOfWork,IPublisher publisher) : IRequestHandler<CreateCategoryCommand>
{
    public async Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var newCategory = new Categories {  CategoryName = request.CategoryName };
        await unitOfWork.Category.AddAsync(newCategory);
        await unitOfWork.SaveAsync();
        await publisher.Publish(new CreateCategoryEvent { CategoryName = request.CategoryName });
    }
}
