namespace DevTalk.Domain.Repositories;

public interface IUnitOfWork
{
    IPostRepository Post { get; }
    Task SaveAsync();
}
