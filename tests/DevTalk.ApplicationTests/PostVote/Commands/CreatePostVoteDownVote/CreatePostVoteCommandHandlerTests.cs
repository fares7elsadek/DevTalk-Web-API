using DevTalk.Application.ApplicationUser;
using DevTalk.Application.PostVote.Commands.CreatePostVoteDownVote;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using Moq;
using System.Linq.Expressions;
using Xunit;


namespace DevTalk.Application.PostVote.Commands.CreatePostVote.Tests;

public class CreatePostVoteCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenPostIdIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var command = new CreatePostDownVoteCommand(null);
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userContextMock = new Mock<IUserContext>();

        var handler = new CreatePostVoteCommandHandler(unitOfWorkMock.Object, userContextMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenPostNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var postId = "valid_post_id";
        var command = new CreatePostDownVoteCommand(postId);

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((Post)null);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);

        var userContextMock = new Mock<IUserContext>();

        var handler = new CreatePostVoteCommandHandler(unitOfWorkMock.Object, userContextMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenUserNotAuthenticated_ThrowsCustomeException()
    {
        // Arrange
        var postId = "valid_post_id";
        var command = new CreatePostDownVoteCommand(postId);

        var post = new Post { PostId = postId, Votes = new List<PostVotes>() };

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(post);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns((CurrentUser)null);

        var handler = new CreatePostVoteCommandHandler(unitOfWorkMock.Object, userContextMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<CustomeException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenVoteDoesNotExist_AddsDownVote()
    {
        // Arrange
        var postId = "valid_post_id";
        var userId = "user_id";
        var command = new CreatePostDownVoteCommand(postId);

        var post = new Post { PostId = postId, Votes = new List<PostVotes>() };

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(post);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser(userId, "user@email.com", [UserRoles.User]));

        var handler = new CreatePostVoteCommandHandler(unitOfWorkMock.Object, userContextMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        Xunit.Assert.Single(post.Votes);
        Xunit.Assert.Equal(VoteType.DownVote, post.Votes.First().VoteType);
        unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenVoteIsAlreadyDownVote_RemovesVote()
    {
        // Arrange
        var postId = "valid_post_id";
        var userId = "user_id";
        var command = new CreatePostDownVoteCommand(postId);

        var existingVote = new PostVotes { UserId = userId, PostId = postId, VoteType = VoteType.DownVote };
        var post = new Post { PostId = postId, Votes = new List<PostVotes> { existingVote } };

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(post);

        var postVotesRepositoryMock = new Mock<IPostVotesRepository>();
        postVotesRepositoryMock.Setup(repo => repo.Remove(It.IsAny<PostVotes>())).Verifiable();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.PostVotes).Returns(postVotesRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser(userId, "user@email.com", [UserRoles.User]));

        var handler = new CreatePostVoteCommandHandler(unitOfWorkMock.Object, userContextMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenVoteIsUpVote_ChangesToDownVote()
    {
        // Arrange
        var postId = "valid_post_id";
        var userId = "user_id";
        var command = new CreatePostDownVoteCommand(postId);

        var existingVote = new PostVotes { UserId = userId, PostId = postId, VoteType = VoteType.UpVote };
        var post = new Post { PostId = postId, Votes = new List<PostVotes> { existingVote } };

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(post);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser(userId, "user@email.com", [UserRoles.User]));

        var handler = new CreatePostVoteCommandHandler(unitOfWorkMock.Object, userContextMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        Xunit.Assert.Equal(VoteType.DownVote, existingVote.VoteType);
        unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}