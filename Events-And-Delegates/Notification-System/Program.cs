class Program
{
    delegate void Notify(string msg);

    static void LogToConsole(string msg)
    {
        Console.WriteLine("[Log] " + msg);
    }

    static void SendEmail(string msg)
    {
        Console.WriteLine("[Email] " + msg);
    }

    static void SendNotification(string msg)
    {
        Console.WriteLine("[Notification] " + msg);
    }

    public static void Main()
    {

        Notify? notify = LogToConsole;

        var msg01 = "Shahed has joined the chat";

        // Subscribe
        notify += SendEmail;
        notify += SendNotification;

        notify(msg01);

        var msg02 = "Demo has left the chat";

        // Unsubscribe
        notify -= SendEmail;
        notify(msg02);


    }
}
