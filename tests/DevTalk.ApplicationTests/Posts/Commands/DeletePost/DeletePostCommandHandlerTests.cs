using Xunit;
using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Services;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Moq;
using System.Linq.Expressions;

namespace DevTalk.Application.Posts.Commands.DeletePost.Tests
{
    public class DeletePostCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WhenPostIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var command = new DeletePostCommand(null);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userContextMock = new Mock<IUserContext>();
            var fileServiceMock = new Mock<IFileService>();
            var publisherMock = new Mock<IPublisher>();

            var handler = new DeletePostCommandHandler(unitOfWorkMock.Object, userContextMock.Object, fileServiceMock.Object, publisherMock.Object);

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenPostNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var postId = "valid_post_id";
            var command = new DeletePostCommand(postId);

            var postRepositoryMock = new Mock<IPostRepository>();
            postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
                .ReturnsAsync((Post)null);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);

            var userContextMock = new Mock<IUserContext>();
            var fileServiceMock = new Mock<IFileService>();
            var publisherMock = new Mock<IPublisher>();

            var handler = new DeletePostCommandHandler(unitOfWorkMock.Object, userContextMock.Object, fileServiceMock.Object, publisherMock.Object);

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenUserNotAuthorized_ThrowsCustomeException()
        {
            // Arrange
            var postId = "valid_post_id";
            var command = new DeletePostCommand(postId);

            var post = new Post { PostId = postId, User = new User { Id = "post_owner_id" } };

            var postRepositoryMock = new Mock<IPostRepository>();
            postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
                .ReturnsAsync(post);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("unauthorized_id", "user@email.com", [UserRoles.User]));

            var fileServiceMock = new Mock<IFileService>();
            var publisherMock = new Mock<IPublisher>();

            var handler = new DeletePostCommandHandler(unitOfWorkMock.Object, userContextMock.Object, fileServiceMock.Object, publisherMock.Object);

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<CustomeException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenUserIsAdmin_DeletesPostSuccessfully()
        {
            // Arrange
            var postId = "valid_post_id";
            var command = new DeletePostCommand(postId);

            var post = new Post { PostId = postId, PostMedias = new List<PostMedia>(), Comments = new List<Comment>(), Votes = new List<PostVotes>(), User = new User { Id = "post_owner_id" } };

            var postRepositoryMock = new Mock<IPostRepository>();
            postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
                .ReturnsAsync(post);

            var postMediaRepositoryMock = new Mock<IPostMediaRepository>();
            postMediaRepositoryMock.Setup(repo => repo.RemoveRange(It.IsAny<IEnumerable<PostMedia>>()))
                .Verifiable();

            var commentRepositoryMock = new Mock<ICommentRepository>();
            commentRepositoryMock.Setup(repo => repo.RemoveRange(It.IsAny<IEnumerable<Comment>>()))
                .Verifiable();

            var postVotesRepositoryMock = new Mock<IPostVotesRepository>();
            postVotesRepositoryMock.Setup(repo => repo.RemoveRange(It.IsAny<IEnumerable<PostVotes>>()))
                .Verifiable();

            var bookMarksRepositoryMock = new Mock<IBookmarkRepository>();
            bookMarksRepositoryMock.Setup(repo => repo.RemoveRange(It.IsAny<IEnumerable<Bookmarks>>()))
                .Verifiable();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.PostMedia).Returns(postMediaRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Comment).Returns(commentRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.PostVotes).Returns(postVotesRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Bookmark).Returns(bookMarksRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser("admin_id", "admin@email.com", [UserRoles.Admin]));

            var fileServiceMock = new Mock<IFileService>();
            var publisherMock = new Mock<IPublisher>();

            var handler = new DeletePostCommandHandler(unitOfWorkMock.Object, userContextMock.Object, fileServiceMock.Object, publisherMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            unitOfWorkMock.Verify(uow => uow.Post.Remove(It.Is<Post>(p => p.PostId == postId)), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            publisherMock.Verify(p => p.Publish(It.Is<DeletePostEvent>(e => e.PostId == postId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenUserIsPostOwner_DeletesPostSuccessfully()
        {
            // Arrange
            var postId = "valid_post_id";
            var userId = "post_owner_id";
            var command = new DeletePostCommand(postId);

            var post = new Post { PostId = postId, PostMedias = new List<PostMedia>(), Comments = new List<Comment>(), Votes = new List<PostVotes>(), User = new User { Id = userId } };

            var postRepositoryMock = new Mock<IPostRepository>();
            postRepositoryMock.Setup(repo => repo.GetOrDefalutAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>()))
                .ReturnsAsync(post);

            var postMediaRepositoryMock = new Mock<IPostMediaRepository>();
            postMediaRepositoryMock.Setup(repo => repo.RemoveRange(It.IsAny<IEnumerable<PostMedia>>()))
                .Verifiable();

            var commentRepositoryMock = new Mock<ICommentRepository>();
            commentRepositoryMock.Setup(repo => repo.RemoveRange(It.IsAny<IEnumerable<Comment>>()))
                .Verifiable();

            var postVotesRepositoryMock = new Mock<IPostVotesRepository>();
            postVotesRepositoryMock.Setup(repo => repo.RemoveRange(It.IsAny<IEnumerable<PostVotes>>()))
                .Verifiable();

            var bookMarksRepositoryMock = new Mock<IBookmarkRepository>();
            bookMarksRepositoryMock.Setup(repo => repo.RemoveRange(It.IsAny<IEnumerable<Bookmarks>>()))
                .Verifiable();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.Post).Returns(postRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.PostMedia).Returns(postMediaRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Comment).Returns(commentRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.PostVotes).Returns(postVotesRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Bookmark).Returns(bookMarksRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser(userId, "user@email.com", [UserRoles.User]));

            var fileServiceMock = new Mock<IFileService>();
            var publisherMock = new Mock<IPublisher>();

            var handler = new DeletePostCommandHandler(unitOfWorkMock.Object, userContextMock.Object, fileServiceMock.Object, publisherMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            unitOfWorkMock.Verify(uow => uow.Post.Remove(It.Is<Post>(p => p.PostId == postId)), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            publisherMock.Verify(p => p.Publish(It.Is<DeletePostEvent>(e => e.PostId == postId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}