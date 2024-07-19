namespace RedditTrackerAPI.Data.Interfaces.Repositories
{
    public interface IRedditRepository
    {
        Task<string> GetAccessTokenAsync();
        Task<string> GetSubredditPostsAsync(string subreddit, string accessToken);
    }
}
