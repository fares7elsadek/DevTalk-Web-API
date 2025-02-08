using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Moq;
using System.Linq.Expressions;
using Xunit;


namespace DevTalk.Application.Comments.Commands.DeleteComment.Tests;

public class DeleteCommentCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenPostIdOrCommentIdIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var command = new DeleteCommentCommand(null,null);
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userContextMock = new Mock<IUserContext>();
        var publisherMock = new Mock<IPublisher>();

        var handler = new DeleteCommentCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenPostNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var postId = "valid_post_id";
        var commentId = "valid_comment_id";
        var command = new DeleteCommentCommand(postId,commentId);

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((Post)null);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);

        var userContextMock = new Mock<IUserContext>();
        var publisherMock = new Mock<IPublisher>();

        var handler = new DeleteCommentCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenCommentNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var postId = "valid_post_id";
        var commentId = "invalid_comment_id";
        var command = new DeleteCommentCommand(commentId,postId);

        var post = new Post { PostId = postId, Comments = new List<Comment>() }; // No comments

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(post);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);

        var userContextMock = new Mock<IUserContext>();
        var publisherMock = new Mock<IPublisher>();

        var handler = new DeleteCommentCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenUserNotAuthorized_ThrowsCustomeException()
    {
        // Arrange
        var postId = "valid_post_id";
        var commentId = "valid_comment_id";
        var command = new DeleteCommentCommand(commentId,postId);

        var post = new Post
        {
            PostId = postId,
            UserId = "post_owner_id",
            Comments = new List<Comment>
        {
            new Comment { CommentId = commentId, UserId = "comment_owner_id" }
        }
        };

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(post);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("unauthroized_id", "user@email.com", [UserRoles.User]));
        
        var publisherMock = new Mock<IPublisher>();

        var handler = new DeleteCommentCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<CustomeException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenUserIsAdmin_DeletesCommentSuccessfully()
    {
        // Arrange
        var postId = "valid_post_id";
        var commentId = "valid_comment_id";
        var command = new DeleteCommentCommand(commentId,postId);

        var post = new Post
        {
            PostId = postId,
            UserId = "post_owner_id",
            Comments = new List<Comment>
        {
            new Comment { CommentId = commentId, UserId = "comment_owner_id" }
        }
        };

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(post);

        var commentRepositoryMock = new Mock<ICommentRepository>();
        commentRepositoryMock.Setup(repo => repo.Remove(It.IsAny<Comment>()));

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.Comment).Returns(commentRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("unauthroized_id", "user@email.com", [UserRoles.Admin]));

        var publisherMock = new Mock<IPublisher>();

        var handler = new DeleteCommentCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        commentRepositoryMock.Verify(repo => repo.Remove(It.Is<Comment>(c => c.CommentId == commentId)), Times.Once);
        unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        publisherMock.Verify(p => p.Publish(It.Is<DeleteCommentEvent>(e =>
            e.CommentId == commentId && e.PostId == postId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserIsCommentOwner_DeletesCommentSuccessfully()
    {
        // Arrange
        var postId = "valid_post_id";
        var commentId = "valid_comment_id";
        var userId = "comment_owner_id";
        var command = new DeleteCommentCommand(commentId,postId);

        var post = new Post
        {
            PostId = postId,
            UserId = "post_owner_id",
            Comments = new List<Comment>
        {
            new Comment { CommentId = commentId, UserId = userId }
        }
        };

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(post);

        var commentRepositoryMock = new Mock<ICommentRepository>();
        commentRepositoryMock.Setup(repo => repo.Remove(It.IsAny<Comment>()));

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.Comment).Returns(commentRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser(userId, "user@email.com", [UserRoles.User]));
       

        var publisherMock = new Mock<IPublisher>();

        var handler = new DeleteCommentCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        commentRepositoryMock.Verify(repo => repo.Remove(It.Is<Comment>(c => c.CommentId == commentId)), Times.Once);
        unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        publisherMock.Verify(p => p.Publish(It.Is<DeleteCommentEvent>(e =>
            e.CommentId == commentId && e.PostId == postId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserIsPostOwner_DeletesCommentSuccessfully()
    {
        // Arrange
        var postId = "valid_post_id";
        var commentId = "valid_comment_id";
        var userId = "post_owner_id";
        var command = new DeleteCommentCommand(commentId,postId);

        var post = new Post
        {
            PostId = postId,
            UserId = userId,
            Comments = new List<Comment>
        {
            new Comment { CommentId = commentId, UserId = "comment_owner_id" }
        }
        };

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(post);

        var commentRepositoryMock = new Mock<ICommentRepository>();
        commentRepositoryMock.Setup(repo => repo.Remove(It.IsAny<Comment>()));

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.Comment).Returns(commentRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser(userId, "user@email.com", [UserRoles.User]));


        var publisherMock = new Mock<IPublisher>();

        var handler = new DeleteCommentCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        commentRepositoryMock.Verify(repo => repo.Remove(It.Is<Comment>(c => c.CommentId == commentId)), Times.Once);
        unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        publisherMock.Verify(p => p.Publish(It.Is<DeleteCommentEvent>(e =>
            e.CommentId == commentId && e.PostId == postId),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}