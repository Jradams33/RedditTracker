using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTrackerAPI.Data.Entities
{
    public class SubredditEntity
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public List<SubredditTopicEntity> Topics { get; set; }

    }
}
