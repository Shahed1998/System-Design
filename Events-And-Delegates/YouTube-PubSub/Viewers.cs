// ══════════════════════════════════════════════
//  SUBSCRIBERS — the viewers
// ══════════════════════════════════════════════

namespace YouTube_PubSub
{
    class Viewer
    {
        public string? Name { get; set; }

        public void OnNotified(string channel, string title)
        {
            Console.WriteLine($"  {Name} got notified : \"{title}\" by {channel}");
        }
    }

    class EmailService
    {
        public void SendEmail(string channel, string title)
        {
            Console.WriteLine($" Email sent : New video \"{title}\" from {channel}");
        }
    }
}
