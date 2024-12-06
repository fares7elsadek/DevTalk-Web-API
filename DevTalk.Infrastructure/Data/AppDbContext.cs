﻿using DevTalk.Domain.Entites;
using DevTalk.Infrastructure.Data.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevTalk.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options):IdentityDbContext<User>(options)
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<PostVotes> PostVotes { get; set; }
    public DbSet<CommentVotes> CommentVotes { get; set; }
    public DbSet<PostMedia> PostMedia { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(PostConfigurations).Assembly);
    }
}
