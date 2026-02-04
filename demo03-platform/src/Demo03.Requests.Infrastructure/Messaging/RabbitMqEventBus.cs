using Demo03.Requests.Application.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Demo03.Requests.Infrastructure.Messaging
{
    public sealed class RabbitMqEventBus : IEventBus, IDisposable
    {
        private readonly RabbitMqOptions _opt;
        private readonly ILogger<RabbitMqEventBus> _logger;
        private readonly IConnection _connection;
        private readonly object _sync = new();

        public RabbitMqEventBus(IOptions<RabbitMqOptions> options, ILogger<RabbitMqEventBus> logger)
        {
            _opt = options.Value;
            _logger = logger;

            var factory = new ConnectionFactory
            {
                HostName = _opt.HostName,
                Port = _opt.Port,
                UserName = _opt.UserName,
                Password = _opt.Password,
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();

            // Creamos el exchange una sola vez
            using var ch = _connection.CreateModel();
            ch.ExchangeDeclare(exchange: _opt.Exchange, type: ExchangeType.Topic, durable: true, autoDelete: false);

            _logger.LogInformation("RabbitMqEventBus connected. Host={Host}:{Port} Exchange={Exchange}",
                _opt.HostName, _opt.Port, _opt.Exchange);
        }

        public Task PublishAsync<T>(T message, CancellationToken ct) where T : class
        {
            // routingKey simple: nombre del tipo (ej: request.submitted)
            var routingKey = ToRoutingKey(typeof(T).Name);

            var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var body = Encoding.UTF8.GetBytes(json);

            lock (_sync)
            {
                using var ch = _connection.CreateModel();

                var props = ch.CreateBasicProperties();
                props.DeliveryMode = 2; // persistente
                props.ContentType = "application/json";
                props.Type = typeof(T).FullName;
                props.MessageId = Guid.NewGuid().ToString("N");
                props.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                ch.BasicPublish(
                    exchange: _opt.Exchange,
                    routingKey: routingKey,
                    basicProperties: props,
                    body: body);

                _logger.LogInformation("Event published. Type={Type} RoutingKey={RoutingKey}", typeof(T).Name, routingKey);
            }

            return Task.CompletedTask;
        }

        private static string ToRoutingKey(string typeName)
        {
            // RequestSubmitted -> request.submitted
            var sb = new StringBuilder();
            for (int i = 0; i < typeName.Length; i++)
            {
                var c = typeName[i];
                if (char.IsUpper(c) && i > 0) sb.Append('.');
                sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString();
        }

        public void Dispose()
        {
            try { _connection?.Close(); } catch { /* ignore */ }
            try { _connection?.Dispose(); } catch { /* ignore */ }
        }
    }
}
