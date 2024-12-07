using DevTalk.Domain.Entites;
using DevTalk.Domain.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace DevTalk.Application.Services;

public interface IAuthService
{
    Task<JwtSecurityToken> CreatJwtToken(User user);
    Task<AuthResponse> GetJwtToken(User user,List<string> roles);
    RefreshToken GenerateRefreshToken();
}
