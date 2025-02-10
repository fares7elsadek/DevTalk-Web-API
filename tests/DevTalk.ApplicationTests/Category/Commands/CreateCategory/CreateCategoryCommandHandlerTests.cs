using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Moq;
using Xunit;


namespace DevTalk.Application.Category.Commands.CreateCategory.Tests;

public class CreateCategoryCommandHandlerTests
{
    
    [Fact]
    public async Task Handle_WhenUserIsAdmin_CreatesCategorySuccessfully()
    {
        // Arrange
        var command = new CreateCategoryCommand { CategoryName = "Tech" };

        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        categoryRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Categories>()))
            .Returns(Task.CompletedTask);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var publisherMock = new Mock<IPublisher>();
        unitOfWorkMock.Setup(uow => uow.Category).Returns(categoryRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser())
            .Returns(new CurrentUser("admin_id", "admin@email.com", new[] { UserRoles.Admin }));

        var handler = new CreateCategoryCommandHandler(unitOfWorkMock.Object, publisherMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        categoryRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Categories>(c => c.CategoryName == "Tech")), Times.Once);
        unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}