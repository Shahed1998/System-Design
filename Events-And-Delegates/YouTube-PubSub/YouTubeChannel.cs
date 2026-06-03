// ══════════════════════════════════════════════
//  PUBLISHER — the YouTube Channel
// ══════════════════════════════════════════════

namespace YouTube_PubSub
{
    internal class YouTubeChannel
    {
        public string? ChannelName { get; set; }

        public delegate void VideoUploadHandler(string channelName, string videoTitle);

        public event VideoUploadHandler? OnVideoUploaded;

        public void UploadVideo(string videoTitle)
        {
            Console.WriteLine($"\n[{ChannelName}] Uploading: \"{videoTitle}\"");
            Console.WriteLine($"[{ChannelName}] Notifying subscribers...\n");
            OnVideoUploaded?.Invoke(ChannelName!, videoTitle);
        }
    }
}
