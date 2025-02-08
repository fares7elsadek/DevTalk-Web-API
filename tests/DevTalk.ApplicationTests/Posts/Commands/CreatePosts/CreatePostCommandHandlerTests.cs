using Xunit;
using DevTalk.Application.Posts.Commands.CreatePosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Application.Services;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;

namespace DevTalk.Application.Posts.Commands.CreatePosts.Tests
{
    public class CreatePostCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WhenUserNotAuthorized_ThrowsCustomeException()
        {
            // Arrange
            var command = new CreatePostCommand
            {
                Title = "Test Title",
                Body = "Test Body",
                Files = null
            };

            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileServiceMock = new Mock<IFileService>();
            var userContextMock = new Mock<IUserContext>();
            var publisherMock = new Mock<IPublisher>();

            userContextMock.Setup(uc => uc.GetCurrentUser()).Returns((CurrentUser)null);

            var handler = new CreatePostCommandHandler(mapperMock.Object, unitOfWorkMock.Object, fileServiceMock.Object, userContextMock.Object, publisherMock.Object);

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<CustomeException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenNoFilesProvided_CreatesPostSuccessfully()
        {
            // Arrange
            var command = new CreatePostCommand
            {
                Title = "Test Title",
                Body = "Test Body",
                Files = null,
                Categories = null
            };
            var user = new CurrentUser("user_id", "user@email.com", new[] { UserRoles.User });
            var post = new Post { Title = command.Title, Body = command.Body, UserId = user.userId };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<PostDto>(It.IsAny<Post>())).Returns(new PostDto());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.Post.AddAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

            var fileServiceMock = new Mock<IFileService>();
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(user);

            var publisherMock = new Mock<IPublisher>();

            var handler = new CreatePostCommandHandler(mapperMock.Object, unitOfWorkMock.Object, fileServiceMock.Object, userContextMock.Object, publisherMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            unitOfWorkMock.Verify(uow => uow.Post.AddAsync(It.Is<Post>(p => p.Title == command.Title && p.Body == command.Body)), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            publisherMock.Verify(p => p.Publish(It.Is<CreatePostsEvent>(e => e.Title == command.Title && e.Body == command.Body), It.IsAny<CancellationToken>()), Times.Once);
            Xunit.Assert.NotNull(result);
        }

        [Fact]
        public async Task Handle_WhenFileCountExceedsLimit_ThrowsCustomeException()
        {
            // Arrange
            var files = new List<IFormFile>();
            for (int i = 0; i < 6; i++)
            {
                var fileMock = new Mock<IFormFile>();
                fileMock.Setup(f => f.Length).Returns(1024);
                files.Add(fileMock.Object);
            }

            var command = new CreatePostCommand
            {
                Title = "Test Title",
                Body = "Test Body",
                Files = files
            };
            var user = new CurrentUser("user_id", "user@email.com", new[] { UserRoles.User });

            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileServiceMock = new Mock<IFileService>();
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(user);

            var publisherMock = new Mock<IPublisher>();

            var handler = new CreatePostCommandHandler(mapperMock.Object, unitOfWorkMock.Object, fileServiceMock.Object, userContextMock.Object, publisherMock.Object);

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<CustomeException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenFileSizeExceedsLimit_ThrowsCustomeException()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(6 * 1024 * 1024); // 6 MB

            var files = new List<IFormFile> { fileMock.Object };
            var command = new CreatePostCommand
            {
                Title = "Test Title",
                Body = "Test Body",
                Files = files
            };
            var user = new CurrentUser("user_id", "user@email.com", new[] { UserRoles.User });

            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileServiceMock = new Mock<IFileService>();
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(user);

            var publisherMock = new Mock<IPublisher>();

            var handler = new CreatePostCommandHandler(mapperMock.Object, unitOfWorkMock.Object, fileServiceMock.Object, userContextMock.Object, publisherMock.Object);

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<CustomeException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenFilesAreValid_CreatesPostWithMediaSuccessfully()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1024);
            fileMock.Setup(f => f.FileName).Returns("image.jpg");

            var files = new List<IFormFile> { fileMock.Object };
            var command = new CreatePostCommand
            {
                Title = "Test Title",
                Body = "Test Body",
                Files = files
            }; ;
            var user = new CurrentUser("user_id", "user@email.com", new[] { UserRoles.User });

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<PostDto>(It.IsAny<Post>())).Returns(new PostDto());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.Post.AddAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(fs => fs.SaveFileAsync(It.IsAny<IFormFile>(), It.IsAny<string[]>())).ReturnsAsync("/path/to/image.jpg");

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(user);

            var publisherMock = new Mock<IPublisher>();

            var handler = new CreatePostCommandHandler(mapperMock.Object, unitOfWorkMock.Object, fileServiceMock.Object, userContextMock.Object, publisherMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            unitOfWorkMock.Verify(uow => uow.Post.AddAsync(It.Is<Post>(p => p.Title == command.Title && p.Body == command.Body && p.PostMedias.Count == 1)), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            publisherMock.Verify(p => p.Publish(It.Is<CreatePostsEvent>(e => e.Title == command.Title && e.Body == command.Body && e.Files == command.Files), It.IsAny<CancellationToken>()), Times.Once);
            Xunit.Assert.NotNull(result);
        }
    }
}