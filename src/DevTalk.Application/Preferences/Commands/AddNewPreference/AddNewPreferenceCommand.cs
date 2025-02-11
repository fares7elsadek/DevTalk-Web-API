using MediatR;

namespace DevTalk.Application.Preferences.Commands.AddNewPreference;

public class AddNewPreferenceCommand(string categoryId):IRequest
{
    public string CategoryId { get; set; } = categoryId;
}
