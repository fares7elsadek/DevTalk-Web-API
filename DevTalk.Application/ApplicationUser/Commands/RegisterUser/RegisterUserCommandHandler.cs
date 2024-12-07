using AutoMapper;
using DevTalk.Application.Services;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace DevTalk.Application.ApplicationUser.Commands.RegisterUser;

public class RegisterUserCommandHandler(UserManager<User> userManager,
    IMapper mapper,IAuthService authService) : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await userManager.FindByEmailAsync(request.Email) is not null)
            return new AuthResponse { Message = "User email already exsit" };

        if (await userManager.FindByNameAsync(request.UserName) is not null)
            return new AuthResponse { Message = "Username already exsit" };

        var user = mapper.Map<User>(request);
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            List<string> errors = new List<string>();
            foreach(var error in result.Errors)
            {
                errors.Add(error.Description);
            }
            throw new CustomeException(string.Join(',',errors));
        }
        await userManager.AddToRoleAsync(user, UserRoles.User);
        var JwtSecurityToken = await authService.CreatJwtToken(user);
        if (JwtSecurityToken == null)
            throw new CustomeException("Somthing wrong has happened");
        return new AuthResponse
        {
            Message = "User registered successfully",
            Email = user.Email,
            ExpiresOne = JwtSecurityToken.ValidTo,
            IsAuthenticated = true,
            Roles = new List<string> { UserRoles.User },
            Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken),
            Username = user.UserName
        };
    }
}
