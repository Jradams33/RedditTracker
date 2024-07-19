using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTrackerAPI.Data.Entities
{
    public class SubredditTopicEntity
    {
        public long Id { get; set; }
        public long SubredditId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public decimal UpvoteRatio { get; set; }
        public int Upvotes { get; set; }
        public DateTime Created {  get; set; }
        public DateTime? Updated { get; set; }
    }
}
