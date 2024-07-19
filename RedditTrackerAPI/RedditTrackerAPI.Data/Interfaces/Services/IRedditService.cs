using RedditTrackerAPI.Data.DTOs;
using RedditTrackerAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTrackerAPI.Data.Interfaces.Services
{
    public interface IRedditService
    {
        Task<SubredditEntity> GetNewSubredditData(string subredditName);
        Task<IEnumerable<SubredditEntity>> GetAllSubredditData();
        Task UpdateSubredditNickname(long subredditId, string newNickname);
        Task DeleteSubreddit(long subredditId);
    }
}
