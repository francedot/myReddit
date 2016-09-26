using System.Collections.Generic;
using System.Threading.Tasks;
using myReddit.Models;

namespace myReddit.Services
{
public interface IRedditApiSource
{
    Task<IList<Subreddit>> GetSubredditsAsync();
    Task<IList<Post>> GetPostsAsync(Subreddit subreddit);
    Task<IList<Comment>> GetCommentsAsync(Post post);
}
}