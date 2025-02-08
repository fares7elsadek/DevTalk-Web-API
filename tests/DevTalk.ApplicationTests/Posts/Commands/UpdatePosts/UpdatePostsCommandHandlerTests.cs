using Xunit;
using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Moq;
using System.Linq.Expressions;

namespace DevTalk.Application.Posts.Commands.UpdatePosts.Tests;

public class UpdatePostsCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenBothTitleAndBodyAreNull_ThrowsCustomeException()
    {
        // Arrange
        var command = new UpdatePostsCommand { PostId = "post_id", Title = null, Body = null };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userContextMock = new Mock<IUserContext>();
        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdatePostsCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<CustomeException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenPostIdIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var command = new UpdatePostsCommand { PostId = null, Title = "New Title", Body = "New Body" };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userContextMock = new Mock<IUserContext>();
        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdatePostsCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenPostNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var postId = "valid_post_id";
        var command = new UpdatePostsCommand { PostId = postId, Title = "New Title", Body = "New Body" };

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((Post)null);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);

        var userContextMock = new Mock<IUserContext>();
        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdatePostsCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenUserNotAuthorized_ThrowsCustomeException()
    {
        // Arrange
        var postId = "valid_post_id";
        var command = new UpdatePostsCommand { PostId = postId, Title = "New Title", Body = "New Body" };

        var post = new Post { PostId = postId, User = new User { Id = "post_owner_id" } };

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(post);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("unauthorized_id", "user@email.com", [UserRoles.User]));

        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdatePostsCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<CustomeException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenUserIsAdmin_UpdatesPostSuccessfully()
    {
        // Arrange
        var postId = "valid_post_id";
        var command = new UpdatePostsCommand { PostId = postId, Title = "Updated Title", Body = "Updated Body" };

        var post = new Post { PostId = postId, Title = "Old Title", Body = "Old Body", User = new User { Id = "post_owner_id" } };

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(post);
        postRepositoryMock.Setup(repo => repo.Update(It.IsAny<Post>())).Verifiable();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("admin_id", "admin@email.com", [UserRoles.Admin]));

        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdatePostsCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        postRepositoryMock.Verify(repo => repo.Update(It.Is<Post>(p => p.Title == "Updated Title" && p.Body == "Updated Body")), Times.Once);
        unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        publisherMock.Verify(p => p.Publish(It.Is<UpdatePostEvent>(e => e.PostId == postId && e.Title == "Updated Title" && e.Body == "Updated Body"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserIsPostOwner_UpdatesPostSuccessfully()
    {
        // Arrange
        var postId = "valid_post_id";
        var userId = "post_owner_id";
        var command = new UpdatePostsCommand { PostId = postId, Title = "Updated Title", Body = "Updated Body" };

        var post = new Post { PostId = postId, Title = "Old Title", Body = "Old Body", User = new User { Id = userId } };

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(post);
        postRepositoryMock.Setup(repo => repo.Update(It.IsAny<Post>())).Verifiable();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser(userId, "user@email.com", [UserRoles.User]));

        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdatePostsCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        postRepositoryMock.Verify(repo => repo.Update(It.Is<Post>(p => p.Title == "Updated Title" && p.Body == "Updated Body")), Times.Once);
        unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        publisherMock.Verify(p => p.Publish(It.Is<UpdatePostEvent>(e => e.PostId == postId && e.Title == "Updated Title" && e.Body == "Updated Body"), It.IsAny<CancellationToken>()), Times.Once);
    }
}