using MediatR;

namespace DevTalk.Application.Preferences.Commands.AddManyPreferences;

public class AddManyPreferencesCommand:IRequest
{
    public HashSet<string> CategoryIds { get; set; } = new HashSet<string>();
}
