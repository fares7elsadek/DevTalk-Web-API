using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Moq;
using System.Linq.Expressions;
using Xunit;
namespace DevTalk.Application.Bookmark.commands.CreateBookmark.Tests;

public class CreateBookmarkCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenUserNotAuthenticated_ThrowsCustomeException()
    {
        // Arrange
        var command = new CreateBookmarkCommand("post_id");

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userContextMock = new Mock<IUserContext>();
        var publisherMock = new Mock<IPublisher>();
        
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns((CurrentUser)null);
        var handler = new CreateBookmarkCommandHandler(unitOfWorkMock.Object, userContextMock.Object,publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<CustomeException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenPostNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new CreateBookmarkCommand("post_id");

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), null))
            .ReturnsAsync((Post)null);

        var userContextMock = new Mock<IUserContext>();
        var publisherMock = new Mock<IPublisher>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("user_id", "user@email.com", []));

        var handler = new CreateBookmarkCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenAppUserNotFound_ThrowsCustomeException()
    {
        // Arrange
        var command = new CreateBookmarkCommand("post_id");

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), null))
            .ReturnsAsync(new Post { PostId = "post_id" });
        unitOfWorkMock.Setup(uow => uow.User.GetOrDefalutAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((User)null);

        var userContextMock = new Mock<IUserContext>();
        var publisherMock = new Mock<IPublisher>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("user_id", "user@email.com", []));

        var handler = new CreateBookmarkCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<CustomeException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenPostAlreadyBookmarked_ThrowsCustomeException()
    {
        // Arrange
        var command = new CreateBookmarkCommand("post_id");

        var post = new Post { PostId = "post_id" };
        var appUser = new User { Id = "user_id", Bookmarks = new List<Bookmarks> { new Bookmarks { PostId = "post_id" } } };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), null))
            .ReturnsAsync(post);
        unitOfWorkMock.Setup(uow => uow.User.GetOrDefalutAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(appUser);

        var userContextMock = new Mock<IUserContext>();
        var publisherMock = new Mock<IPublisher>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("user_id", "user@email.com", []));

        var handler = new CreateBookmarkCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<CustomeException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenValid_AddsBookmarkSuccessfully()
    {
        // Arrange
        var command = new CreateBookmarkCommand("post_id");

        var post = new Post { PostId = "post_id" };
        var appUser = new User { Id = "user_id", Bookmarks = new List<Bookmarks>() };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var publisherMock = new Mock<IPublisher>();
        unitOfWorkMock.Setup(uow => uow.Post.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), null))
            .ReturnsAsync(post);
        unitOfWorkMock.Setup(uow => uow.User.GetOrDefalutAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(appUser);
        unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("user_id", "user@email.com", []));

        var handler = new CreateBookmarkCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        Xunit.Assert.Contains(appUser.Bookmarks, b => b.PostId == "post_id" && b.UserId == "user_id");
        unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}