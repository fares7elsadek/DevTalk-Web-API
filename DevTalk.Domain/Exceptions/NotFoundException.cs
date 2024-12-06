namespace DevTalk.Domain.Exceptions;

public class NotFoundException(string resourceType,string resourceIdentitfier)
    :Exception($"{resourceType} with id {resourceIdentitfier} does not exsit")
{
}
