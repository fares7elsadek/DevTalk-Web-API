using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Moq;
using System.Linq.Expressions;
using Xunit;


namespace DevTalk.Application.Category.Commands.DeleteCategory.Tests;

public class DeleteCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenUserNotAuthorized_ThrowsCustomeException()
    {
        // Arrange
        var command = new DeleteCategoryCommand("test_category_id");

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var publisherMock = new Mock<IPublisher>();

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("user_id", "user@email.com", [UserRoles.User]));

        var handler = new DeleteCategoryCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<CustomeException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenCategoryNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new DeleteCategoryCommand("test_category_id");

        

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var publisherMock = new Mock<IPublisher>();
        unitOfWorkMock.Setup(uow => uow.Category.GetOrDefalutAsync(It.IsAny<Expression<Func<Categories, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((Categories)null);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("admin_id", "admin@email.com", [UserRoles.Admin]));

        var handler = new DeleteCategoryCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenUserIsAdmin_DeletesCategorySuccessfully()
    {
        // Arrange
        var categoryId = "test_category_id";
        var command = new DeleteCategoryCommand("test_category_id");

        var category = new Categories { CategoryId = categoryId, CategoryName = "Test Category" };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var publisherMock = new Mock<IPublisher>();
        unitOfWorkMock.Setup(uow => uow.Category.GetOrDefalutAsync(It.IsAny<Expression<Func<Categories, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(category);

        unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("admin_id", "admin@email.com", [UserRoles.Admin]));

        var handler = new DeleteCategoryCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        unitOfWorkMock.Verify(uow => uow.Category.Remove(It.Is<Categories>(c => c.CategoryId == categoryId)), Times.Once);
        unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}