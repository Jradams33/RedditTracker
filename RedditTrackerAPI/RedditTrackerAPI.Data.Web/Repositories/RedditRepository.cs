using Microsoft.Extensions.Options;
using RedditTrackerAPI.Data.DTOs;
using RedditTrackerAPI.Data.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedditTrackerAPI.Data.Web.Repositories
{
    public class RedditRepository : IRedditRepository
    {
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;

        public RedditRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_appSettings.RedditApiAppId}:{_appSettings.RedditApiAppSecret}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MockClient/0.1 by Me");

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", _appSettings.RedditApiUser),
                new KeyValuePair<string, string>("password", _appSettings.RedditApiPass)
            });

            var response = await _httpClient.PostAsync("https://www.reddit.com/api/v1/access_token", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<JsonElement>(responseString);

            return responseJson.GetProperty("access_token").GetString();
        }

        public async Task<string> GetSubredditPostsAsync(string subreddit, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MockClient/0.1 by Me");

            var response = await _httpClient.GetAsync($"https://oauth.reddit.com/r/{subreddit}/hot");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
