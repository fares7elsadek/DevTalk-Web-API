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

        var handler = new UpdatePostsCommandHandler(unitOfWorkMock.Object, publisherMock.Object);

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

        var handler = new UpdatePostsCommandHandler(unitOfWorkMock.Object, publisherMock.Object);

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

        var handler = new UpdatePostsCommandHandler(unitOfWorkMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    

    
}