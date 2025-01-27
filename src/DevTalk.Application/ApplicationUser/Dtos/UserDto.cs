namespace DevTalk.Application.ApplicationUser.Dtos;

public class UserDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string? Avatar { get; set; }
}
