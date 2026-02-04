namespace Demo03.Notifications.Worker.Options
{
    public sealed class RabbitMqOptions
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string Exchange { get; set; } = "demo03.events";
        public string Queue { get; set; } = "demo03.notifications";
        public string RoutingKey { get; set; } = "request.#";
    }
}
