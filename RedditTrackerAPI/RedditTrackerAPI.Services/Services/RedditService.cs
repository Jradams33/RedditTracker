using RedditTrackerAPI.Data.DTOs;
using RedditTrackerAPI.Data.Entities;
using RedditTrackerAPI.Data.Interfaces.Repositories;
using RedditTrackerAPI.Data.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace RedditTrackerAPI.Services.Services
{
    public class RedditService : IRedditService
    {
        private readonly IRedditRepository _redditRepository;
        private readonly ISubredditRepository _subredditRepository;

        public RedditService(IRedditRepository redditRepository, ISubredditRepository subredditRepository)
        {
            _redditRepository = redditRepository;
            _subredditRepository = subredditRepository;
        }

        public async Task<SubredditEntity> GetNewSubredditData(string subredditName)
        {
            var response = new List<RedditAPIResponse>();

            var accessToken = await _redditRepository.GetAccessTokenAsync();

            var jsonResponse = await _redditRepository.GetSubredditPostsAsync(subredditName, accessToken) ?? string.Empty;

            var jsonResponseObject = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

            // Extract the nested children element
            JsonElement dataElement = jsonResponseObject.GetProperty("data");
            JsonElement childrenElement = dataElement.GetProperty("children");

            foreach (var childElement in childrenElement.EnumerateArray())
            {
                var topicDataElement = childElement.GetProperty("data");
                var topicData = JsonSerializer.Deserialize<RedditAPIResponse>(topicDataElement.GetRawText());
                response.Add(topicData);
            }

            var top3 = response.Where(topic => !topic.Over18).Take(3).ToList();

            // save new entry
            var newSubreddit = new SubredditEntity()
            {
                Nickname = subredditName,
                Name = subredditName,
                Topics = top3.Select(topic => new SubredditTopicEntity
                {
                    Title = topic.Title,
                    Author = topic.Author,
                    Url = topic.PostUrl,
                    UpvoteRatio = topic.UpvoteRatio,
                    Upvotes = topic.Upvotes,
                }).ToList()
            };

            await _subredditRepository.InsertNewSubreddit(newSubreddit);

            return newSubreddit;
        }

        public async Task<IEnumerable<SubredditEntity>> GetAllSubredditData()
        {
            return await _subredditRepository.GetAllSubredditsWithTopicsAsync();
        }

        public async Task UpdateSubredditNickname(long subredditId, string newNickname)
        {
            await _subredditRepository.UpdateSubredditNicknameAsync(subredditId, newNickname);
        }

        public async Task DeleteSubreddit(long subredditId)
        {
            await _subredditRepository.DeleteSubredditAndTopicsAsync(subredditId);
        }
    }
}
