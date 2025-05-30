﻿using DevTalk.Application.ApplicationUser;
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

        var handler = new DeleteCommentCommandHandler(unitOfWorkMock.Object, publisherMock.Object);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
    }

  
    
}