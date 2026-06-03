using YouTube_PubSub;

class Program
{
    static void Main()
    {
        YouTubeChannel channel = new YouTubeChannel
        {
            ChannelName = "CodeWithAhmed"
        };

        // Create subscribers
        Viewer sara = new Viewer { Name = "Sara" };
        Viewer john = new Viewer { Name = "John" };
        Viewer fatima = new Viewer { Name = "Fatima" };
        EmailService email = new EmailService();

        // Subscribe to the event — like hitting the 🔔 button
        channel.OnVideoUploaded += sara.OnNotified;
        channel.OnVideoUploaded += john.OnNotified;
        channel.OnVideoUploaded += fatima.OnNotified;
        channel.OnVideoUploaded += email.SendEmail;

        // Upload video 1 — all 4 get notified
        channel.UploadVideo("C# Events and Delegates Explained");

        channel.OnVideoUploaded -= john.OnNotified; // john unsubscribed from the event

        Console.WriteLine("============================================");

        channel.UploadVideo("Pub Sub explained");
    }
}