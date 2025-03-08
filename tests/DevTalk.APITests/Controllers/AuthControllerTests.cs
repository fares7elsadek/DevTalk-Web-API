using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DevTalk.Application.ApplicationUser.Commands.RegisterUser;
using System.Net;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using DevTalk.Domain.Entites;

namespace DevTalk.API.Controllers.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthController _authController;
        private readonly Mock<SignInManager<User>> _signInManagerMock;

        public AuthControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _authController = new AuthController(_mediatorMock.Object, _signInManagerMock.Object);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnOk_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                Email = "test@example.com",
                UserName = "testuser",
                Password = "Password123!"
            };

            var authResponse = new AuthResponse
            {
                Email = command.Email,
                Username = command.UserName,
                IsAuthenticated = true,
                Message = "User registered successfully. Please check your email to confirm your account."
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(authResponse);

            var apiResponse = new
            {
                IsSuccess = true,
                Errors = (string)null,
                StatusCode = HttpStatusCode.OK,
                Result = new { message = authResponse.Message, email = authResponse.Email }
            };

            // Act
            var result = await _authController.RegisterUser(command) as OkObjectResult;

            // Xunit.Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
