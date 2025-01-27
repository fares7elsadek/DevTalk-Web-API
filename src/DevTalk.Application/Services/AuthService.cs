using DevTalk.Domain.Entites;
using DevTalk.Domain.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DevTalk.Application.Services;

public class AuthService : IAuthService
{
    private readonly JwtOptions jwt;
    private readonly UserManager<User> userManager;
    public AuthService(IOptions<JwtOptions> Jwt,
        UserManager<User> userManager)
    {
        jwt = Jwt.Value;
        this.userManager = userManager;
    }
    public async Task<JwtSecurityToken> CreatJwtToken(User user)
    {
        var userClaim = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();
        foreach (var role in roles)
            roleClaims.Add(new Claim("role", role));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email,user.Email),
            new Claim("uid",user.Id)
        }
        .Union(userClaim)
        .Union(roleClaims);

        var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
        var SigningCredentials = new SigningCredentials(symetricSecurityKey,SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer:jwt.Issure,
            audience:jwt.Audience,
            claims:claims,
            expires:DateTime.Now.AddMinutes(jwt.DurationInMinutes),
            signingCredentials: SigningCredentials
            );
        return jwtSecurityToken;
    }

    public RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        var token = Convert.ToBase64String(randomNumber);
        return new RefreshToken
        {
            Token= token,
            ExpiresOn = DateTime.UtcNow.AddDays(10),
            CreatedOn = DateTime.UtcNow,
        };
    }

    public async Task<AuthResponse> GetJwtToken(User user,List<string> roles)
    {
        AuthResponse authResponse = new AuthResponse();
        var jwtSecurityToken = await CreatJwtToken(user);
        authResponse.Message = "User logged in succeefully";
        authResponse.IsAuthenticated = true;
        authResponse.Email = user.Email;
        authResponse.Username = user.UserName;
        authResponse.Roles = roles;
        authResponse.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        if(user.RefreshTokens.Any(x => x.IsActive))
        {
            var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            authResponse.RefreshToken = activeRefreshToken.Token;
            authResponse.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
        }
        else
        {
            var token = GenerateRefreshToken();
            authResponse.RefreshToken = token.Token;
            authResponse.RefreshTokenExpiration = token.ExpiresOn;
            user.RefreshTokens.Add(token);
            await userManager.UpdateAsync(user);
        }
        return authResponse;
    }
}
