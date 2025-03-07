using DevTalk.Domain.Helpers;

namespace DevTalk.Application.Attributes;

public class HasPermissionAttribute(Permissions permission):Attribute
{
    public Permissions Permission { get; set; } = permission;
}
