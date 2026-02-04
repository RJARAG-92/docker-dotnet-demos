namespace Demo03.Notifications.Worker.Options
{
    public sealed class SmtpOptions
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 1025;
        public string From { get; set; } = "noreply@demo03.local";
        public string To { get; set; } = "test@demo03.local";
    }
}
