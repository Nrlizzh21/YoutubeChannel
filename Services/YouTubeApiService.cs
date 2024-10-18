using YoutubeChannel.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;

namespace YoutubeChannel.Services
{
    public class YouTubeApiService
    {
        private readonly string _apiKey;

        // Constructor to get API key from appsettings.json
        public YouTubeApiService(IConfiguration configuration)
        {
            _apiKey = configuration["YouTubeApiKey"]; // Fetch API key from appsettings.json
        }

        public async Task<(List<YoutubeChannelModel> Items, string NextPageToken, string PreviousPageToken)> SearchChannelAsync(string query, string pageToken = null)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _apiKey,
                ApplicationName = "YoutubeChannel"
            });

            var searchRequest = youtubeService.Search.List("snippet");
            searchRequest.Q = query;
            searchRequest.Type = "channel";
            searchRequest.MaxResults = 15; // Results per page
            searchRequest.PageToken = pageToken; // Page token for pagination

            var searchResponse = await searchRequest.ExecuteAsync();

            var channels = searchResponse.Items
                .Where(item => item.Id.Kind == "youtube#channel")
                .Select(item => new YoutubeChannelModel()
                {
                    ChannelTitle = item.Snippet.Title,
                    ThumbnailUrl = item.Snippet.Thumbnails?.Medium?.Url ?? string.Empty,
                    ChannelUrl = "https://www.youtube.com/channel/" + item.Id.ChannelId,
                })
                .ToList();

            // Return channels along with pagination tokens
            return (channels, searchResponse.NextPageToken, searchResponse.PrevPageToken); 
        }




        public async Task<YoutubeChannelModel> GetChannelDetailsAsync(string channelId)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _apiKey,
                ApplicationName = "YoutubeChannel"
            });

            var channelsRequest = youtubeService.Channels.List("snippet,statistics");
            channelsRequest.Id = channelId;

            var channelsResponse = await channelsRequest.ExecuteAsync();
            var channel = channelsResponse.Items.FirstOrDefault();

            if (channel != null)
            {
                return new YoutubeChannelModel
                {
                    ChannelTitle = channel.Snippet.Title,
                    ChannelDescription = channel.Snippet.Description,
                    ThumbnailUrl = channel.Snippet.Thumbnails?.Medium?.Url ?? string.Empty,
                    ChannelUrl = "https://www.youtube.com/channel/" + channel.Id,
                    PublishedAt = channel.Snippet.PublishedAt,
                    SubscriberCount = channel.Statistics.SubscriberCount ?? 0,
                    VideoCount = channel.Statistics.VideoCount ?? 0,
                    ChannelId = channel.Id, // 
                    CustomUrl = channel.Snippet.CustomUrl // 
                };
            }

            return null;
        }
    }
}
