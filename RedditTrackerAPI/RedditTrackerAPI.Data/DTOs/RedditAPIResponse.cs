using System.Text.Json.Serialization;

namespace RedditTrackerAPI.Data.DTOs
{
    public class RedditAPIResponse
    {
        [JsonPropertyName("url")]
        public string PostUrl { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("upvote_ratio")]
        public decimal UpvoteRatio { get; set; }

        [JsonPropertyName("ups")]
        public int Upvotes { get; set; }

        [JsonPropertyName("over_18")]
        public bool Over18 { get; set; }
    }
}
