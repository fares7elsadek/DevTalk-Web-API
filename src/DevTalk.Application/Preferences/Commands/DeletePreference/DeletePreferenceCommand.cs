using MediatR;

namespace DevTalk.Application.Preferences.Commands.DeletePreference;

public class DeletePreferenceCommand(string categoryId):IRequest
{
    public string CategoryId { get; set; } = categoryId;
}
