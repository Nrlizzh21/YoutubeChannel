using System;

namespace YoutubeChannel.Models
{
    public class YoutubeChannelModel
    {
        public string ChannelTitle { get; set; }

        public string ChannelDescription { get; set; }

        public string ThumbnailUrl { get; set; }

        public string ChannelUrl { get; set; }

        public DateTime? PublishedAt { get; set; }

        public ulong SubscriberCount { get; set; }

        public ulong VideoCount { get; set; }

        public string ChannelId { get; set; }

        public string CustomUrl { get; set; }

    }
}
