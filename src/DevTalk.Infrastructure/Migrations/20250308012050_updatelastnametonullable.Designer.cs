﻿// <auto-generated />
using System;
using DevTalk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevTalk.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250308012050_updatelastnametonullable")]
    partial class updatelastnametonullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DevTalk.Domain.Entites.Bookmarks", b =>
                {
                    b.Property<string>("BookmarkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasDefaultValueSql("newid()");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("PostId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BookmarkId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Bookmarks");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Categories", b =>
                {
                    b.Property<string>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Comment", b =>
                {
                    b.Property<string>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CommentedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("PostId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.CommentVotes", b =>
                {
                    b.Property<string>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("VoteId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VoteType")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("UserId");

                    b.ToTable("CommentVotes");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Notifications", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Timestamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Post", b =>
                {
                    b.Property<string>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("PopularityScore")
                        .HasColumnType("float");

                    b.Property<DateTime>("PostedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PostId");

                    b.HasIndex("UserId");

                    b.HasIndex("PopularityScore", "PostedAt", "PostId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.PostCategory", b =>
                {
                    b.Property<string>("PostId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PostId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("PostCategory");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.PostMedia", b =>
                {
                    b.Property<string>("PostMediaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("MediaFileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MediaUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("PostMediaId");

                    b.HasIndex("PostId");

                    b.ToTable("PostMedia");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.PostVotes", b =>
                {
                    b.Property<string>("VoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("PostId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("VoteType")
                        .HasColumnType("int");

                    b.HasKey("VoteId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("PostVotes");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Preference", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Preferences");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("AvatarFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("BirthOfDate")
                        .HasColumnType("date");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar");

                    b.Property<DateTime?>("ForgetPasswordResetLinkRequestedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastEmailConfirmationToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Bookmarks", b =>
                {
                    b.HasOne("DevTalk.Domain.Entites.Post", "Post")
                        .WithMany("Bookmarks")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DevTalk.Domain.Entites.User", "User")
                        .WithMany("Bookmarks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Comment", b =>
                {
                    b.HasOne("DevTalk.Domain.Entites.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DevTalk.Domain.Entites.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.CommentVotes", b =>
                {
                    b.HasOne("DevTalk.Domain.Entites.Comment", "Comment")
                        .WithMany("Votes")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevTalk.Domain.Entites.User", "User")
                        .WithMany("CommentsVotes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Notifications", b =>
                {
                    b.HasOne("DevTalk.Domain.Entites.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Post", b =>
                {
                    b.HasOne("DevTalk.Domain.Entites.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.PostCategory", b =>
                {
                    b.HasOne("DevTalk.Domain.Entites.Categories", null)
                        .WithMany("PostCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevTalk.Domain.Entites.Post", null)
                        .WithMany("PostCategories")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.PostMedia", b =>
                {
                    b.HasOne("DevTalk.Domain.Entites.Post", "Post")
                        .WithMany("PostMedias")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.PostVotes", b =>
                {
                    b.HasOne("DevTalk.Domain.Entites.Post", "Post")
                        .WithMany("Votes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DevTalk.Domain.Entites.User", "User")
                        .WithMany("PostVotes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Preference", b =>
                {
                    b.HasOne("DevTalk.Domain.Entites.Categories", "Category")
                        .WithMany("Preferences")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DevTalk.Domain.Entites.User", "User")
                        .WithMany("Preferences")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.User", b =>
                {
                    b.OwnsMany("DevTalk.Domain.Entites.RefreshToken", "RefreshTokens", b1 =>
                        {
                            b1.Property<string>("UserId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<DateTime>("CreatedOn")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("ExpiresOn")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime?>("RevokedOn")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Token")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserId", "Id");

                            b1.ToTable("RefreshToken");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DevTalk.Domain.Entites.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DevTalk.Domain.Entites.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevTalk.Domain.Entites.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DevTalk.Domain.Entites.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Categories", b =>
                {
                    b.Navigation("PostCategories");

                    b.Navigation("Preferences");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Comment", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.Post", b =>
                {
                    b.Navigation("Bookmarks");

                    b.Navigation("Comments");

                    b.Navigation("PostCategories");

                    b.Navigation("PostMedias");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("DevTalk.Domain.Entites.User", b =>
                {
                    b.Navigation("Bookmarks");

                    b.Navigation("Comments");

                    b.Navigation("CommentsVotes");

                    b.Navigation("Notifications");

                    b.Navigation("PostVotes");

                    b.Navigation("Posts");

                    b.Navigation("Preferences");
                });
#pragma warning restore 612, 618
        }
    }
}
