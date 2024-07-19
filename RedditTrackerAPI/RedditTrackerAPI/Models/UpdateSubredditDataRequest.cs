namespace RedditTrackerAPI.Models
{
    public class UpdateSubredditDataRequest
    {
        public long SubredditId { get; set; }
        public string SubredditNickName { get; set; }
    }
}
