using RedditTrackerAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTrackerAPI.Data.Interfaces.Repositories
{
    public interface ISubredditRepository
    {
        Task InsertNewSubreddit(SubredditEntity subredditEntity);
        Task<IEnumerable<SubredditEntity>> GetAllSubredditsWithTopicsAsync();
        Task UpdateSubredditNicknameAsync(long subredditId, string newNickname);
        Task DeleteSubredditAndTopicsAsync(long subredditId);
    }
}
