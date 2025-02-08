using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Comments.Commands.CreateComment;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace DevTalk.ApplicationTests.Comments.Commands.CreateComment;

public class CreateCommentCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenPostIdIsNull_ThrowsArgumentNullException()
    {
        // arrange
        var commmand = new CreateCommentCommand(null)
        {
            CommentText = "Test"
        };
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userContextMock = new Mock<IUserContext>();
        var publisherMock = new Mock<IPublisher>();

        var handler = new CreateCommentCommandHandler(unitOfWorkMock.Object, userContextMock.Object,
            publisherMock.Object);
        // act & assert
        await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(commmand,CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenPostNotFound_ThrowsNotFoundException()
    {
        // arragne 
        string postId = "not_found_id";
        var commmand = new CreateCommentCommand(postId)
        {
            CommentText = "Test"
        };
        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(),
            It.IsAny<string>())).ReturnsAsync((Post)null);
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("user123", "user@gmail.com", ["admin"]));
        var publisherMock = new Mock<IPublisher>();

        var handler = new CreateCommentCommandHandler(unitOfWorkMock.Object,userContextMock.Object,publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(commmand, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithValidRequest_CreatesCommentAndSaves()
    {
        // arragne 
        string postId = "exist_postid";
        var commmand = new CreateCommentCommand(postId)
        {
            CommentText = "Test"
        };
        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(),
            It.IsAny<string>())).ReturnsAsync(new Post { PostId=postId });

        var commentRepositoryMock = new Mock<ICommentRepository>();
        commentRepositoryMock.Setup(crm => crm.AddAsync(It.IsAny<Comment>()))
            .Returns(Task.CompletedTask);
        

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.Comment).Returns(commentRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("user123", "user@gmail.com", ["admin"]));
        var publisherMock = new Mock<IPublisher>();

        var handler = new CreateCommentCommandHandler(unitOfWorkMock.Object, userContextMock.Object, publisherMock.Object);

        // Act
        await handler.Handle(commmand, CancellationToken.None);

        // Assert
        commentRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Comment>(c =>
        c.CommentText == "Test" &&
        c.UserId == "user123" &&
        c.PostId == postId)), Times.Once);

        unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);

        publisherMock.Verify(p => p.Publish(It.Is<CreateCommentEvent>(e =>
            e.PostId == postId &&
            e.CommentText == "Test"),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
