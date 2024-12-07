﻿using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    public IPostRepository Post { get; private set; }

    public ICommentRepository Comment { get; private set; }

    public ICommentVotesRepository CommentVotes { get; private set; }

    public IPostMediaRepository PostMedia { get; private set; }

    public IPostVotesRepository PostVotes { get; private set; }

    public IUserRepository User { get; private set; }

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
        Post = new PostRepository(_db);
        Comment = new CommendRepository(_db);
        CommentVotes = new CommentVotesRepository(_db);
        PostMedia = new PostMediaRepository(_db);
        User = new UserRepository(_db);
        PostVotes = new PostVotesRepository(_db);
    }
    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}
