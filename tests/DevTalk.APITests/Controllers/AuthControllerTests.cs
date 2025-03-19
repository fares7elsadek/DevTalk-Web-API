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

        
    }
}
