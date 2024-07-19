using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedditTrackerAPI.Data.DTOs;
using RedditTrackerAPI.Data.Entities;
using RedditTrackerAPI.Data.Interfaces.Services;
using RedditTrackerAPI.Models;

namespace RedditTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubredditController : ControllerBase
    {
        private readonly IRedditService _redditService;

        public SubredditController(IRedditService redditService)
        {
            _redditService = redditService;
        }

        [HttpPost("CreateNewSubreddit")]
        public async Task<ActionResult<SubredditEntity>> CreateNewSubredditData([FromBody] CreateNewSubredditRequest request)
        {
            var data = await _redditService.GetNewSubredditData(request.SubredditName);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubredditEntity>>> GetSubredditData()
        {
            var data = await _redditService.GetAllSubredditData();
            return Ok(data);
        }

        [HttpPut]
        public async Task<ActionResult<SubredditEntity>> UpdateSubredditData([FromBody] UpdateSubredditDataRequest request)
        {
            await _redditService.UpdateSubredditNickname(request.SubredditId, request.SubredditNickName);
            return Ok();
        }

        [HttpDelete("{subredditId}")]
        public async Task<ActionResult<SubredditEntity>> DeleteSubredditData(long subredditId)
        {
            await _redditService.DeleteSubreddit(subredditId);
            return Ok();
        }
    }
}
