using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RedditTrackerAPI.Data.DTOs;
using RedditTrackerAPI.Data.Entities;
using RedditTrackerAPI.Data.Interfaces.Repositories;
using System.Data;

namespace RedditTrackerAPI.Data.Sql.Repositories
{
    public class SubredditRepository : ISubredditRepository
    {
        private readonly AppSettings _appSettings;

        public SubredditRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task InsertNewSubreddit(SubredditEntity subredditEntity)
        {
            using (SqlConnection connection = new SqlConnection(_appSettings.ConnString))
            {
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        long subredditId;

                        using (SqlCommand command = new SqlCommand("INSERT INTO SubredditEntity (Nickname, Name, Created) VALUES (@Nickname, @Name, @Created); SELECT SCOPE_IDENTITY();", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Nickname", subredditEntity.Nickname);
                            command.Parameters.AddWithValue("@Name", subredditEntity.Name);
                            command.Parameters.AddWithValue("@Created", DateTime.UtcNow);

                            subredditId = Convert.ToInt64(await command.ExecuteScalarAsync());
                        }

                        foreach (var topic in subredditEntity.Topics)
                        {
                            using (SqlCommand topicCommand = new SqlCommand("INSERT INTO SubredditTopicEntity (SubredditId, Title, Author, Url, UpvoteRatio, Upvotes, Created) VALUES (@SubredditId, @Title, @Author, @Url, @UpvoteRatio, @Upvotes, @Created)", connection, transaction))
                            {
                                topicCommand.Parameters.AddWithValue("@SubredditId", subredditId);
                                topicCommand.Parameters.AddWithValue("@Title", topic.Title);
                                topicCommand.Parameters.AddWithValue("@Author", topic.Author);
                                topicCommand.Parameters.AddWithValue("@Url", topic.Url ?? (object)DBNull.Value); // Handle null URL
                                topicCommand.Parameters.AddWithValue("@UpvoteRatio", topic.UpvoteRatio);
                                topicCommand.Parameters.AddWithValue("@Upvotes", topic.Upvotes);
                                topicCommand.Parameters.AddWithValue("@Created", DateTime.UtcNow);

                                await topicCommand.ExecuteNonQueryAsync();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An error occurred while inserting the subreddit and topics.", ex);
                    }
                }
            }
        }

        public async Task<IEnumerable<SubredditEntity>> GetAllSubredditsWithTopicsAsync()
        {
            var subreddits = new Dictionary<long, SubredditEntity>();

            using (SqlConnection connection = new SqlConnection(_appSettings.ConnString))
            {
                await connection.OpenAsync();

                string query = @"
        SELECT 
            s.Id AS SubredditId,
            s.Nickname,
            s.Name,
            s.Created AS SubredditCreated,
            s.Updated AS SubredditUpdated,
            t.Id AS TopicId,
            t.Title,
            t.Author,
            t.Url,
            t.UpvoteRatio,
            t.Upvotes,
            t.Created AS TopicCreated,
            t.Updated AS TopicUpdated
        FROM 
            SubredditEntity s
        INNER JOIN 
            SubredditTopicEntity t ON s.Id = t.SubredditId
        ORDER BY s.Created DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var subredditId = reader.GetInt64(reader.GetOrdinal("SubredditId"));

                            if (!subreddits.TryGetValue(subredditId, out var subreddit))
                            {
                                subreddit = new SubredditEntity
                                {
                                    Id = subredditId,
                                    Nickname = reader.GetString(reader.GetOrdinal("Nickname")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Created = reader.GetDateTime(reader.GetOrdinal("SubredditCreated")),
                                    Updated = reader.IsDBNull(reader.GetOrdinal("SubredditUpdated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("SubredditUpdated")),
                                    Topics = new List<SubredditTopicEntity>()
                                };

                                subreddits[subredditId] = subreddit;
                            }

                            var topic = new SubredditTopicEntity
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("TopicId")),
                                SubredditId = subredditId,
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Author = reader.GetString(reader.GetOrdinal("Author")),
                                Url = reader.IsDBNull(reader.GetOrdinal("Url")) ? null : reader.GetString(reader.GetOrdinal("Url")),
                                UpvoteRatio = reader.GetDecimal(reader.GetOrdinal("UpvoteRatio")),
                                Upvotes = reader.GetInt32(reader.GetOrdinal("Upvotes")),
                                Created = reader.GetDateTime(reader.GetOrdinal("TopicCreated")),
                                Updated = reader.IsDBNull(reader.GetOrdinal("TopicUpdated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("TopicUpdated"))
                            };

                            subreddit.Topics.Add(topic);
                        }
                    }
                }
            }

            return subreddits.Values;
        }


        public async Task UpdateSubredditNicknameAsync(long subredditId, string newNickname)
        {
            using (SqlConnection connection = new SqlConnection(_appSettings.ConnString))
            {
                await connection.OpenAsync();

                // Update SubredditEntity
                using (SqlCommand updateSubredditCommand = new SqlCommand(
                    "UPDATE SubredditEntity SET Nickname = @Nickname, Updated = @Updated WHERE Id = @Id", connection))
                {
                    updateSubredditCommand.Parameters.AddWithValue("@Id", subredditId);
                    updateSubredditCommand.Parameters.AddWithValue("@Nickname", newNickname);
                    updateSubredditCommand.Parameters.AddWithValue("@Updated", DateTime.UtcNow);

                    int rowsAffected = await updateSubredditCommand.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("SubredditEntity not found or update failed.");
                    }
                }
            }
        }


        public async Task DeleteSubredditAndTopicsAsync(long subredditId)
        {
            using (SqlConnection connection = new SqlConnection(_appSettings.ConnString))
            {
                await connection.OpenAsync();

                using (SqlCommand deleteTopicsCommand = new SqlCommand(
                    "DELETE FROM SubredditTopicEntity WHERE SubredditId = @SubredditId", connection))
                {
                    deleteTopicsCommand.Parameters.AddWithValue("@SubredditId", subredditId);

                    int rowsAffected = await deleteTopicsCommand.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        // Optionally handle cases where no topics were found
                    }
                }

                // Delete SubredditEntity
                using (SqlCommand deleteSubredditCommand = new SqlCommand(
                    "DELETE FROM SubredditEntity WHERE Id = @Id", connection))
                {
                    deleteSubredditCommand.Parameters.AddWithValue("@Id", subredditId);

                    int rowsAffected = await deleteSubredditCommand.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("SubredditEntity not found or deletion failed.");
                    }
                }
            }
        }

    }
}
