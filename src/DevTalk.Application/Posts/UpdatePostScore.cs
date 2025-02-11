using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.Posts
{
    public static class UpdatePostScore
    {
        public static double UpdateScore(Post post)
        {
            return (post.Votes.Count(v => v.VoteType == VoteType.UpVote) * 2) +
                (post.Comments.Count) -
                (post.Votes.Count(v => v.VoteType == VoteType.DownVote) * 1.5);
        }
    }
}
