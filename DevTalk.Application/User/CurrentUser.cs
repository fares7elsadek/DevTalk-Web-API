namespace DevTalk.Application.User;

public class CurrentUser(string userId , string Email ,IEnumerable<string> Roles)
{
    public string userId = userId;
    public string Email = Email;
    public bool IsInRole(string role) => Roles.Contains(role);
}
