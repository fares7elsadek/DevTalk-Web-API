using DevTalk.Domain.Entites;
using System.IdentityModel.Tokens.Jwt;

namespace DevTalk.Application.Services;

public interface IAuthService
{
    Task<JwtSecurityToken> CreatJwtToken(User user);
}
