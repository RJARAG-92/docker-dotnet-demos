using Demo03.Notifications.Worker.Options;
using Demo03.Notifications.Worker.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Demo03.Notifications.Worker.Workers
{
    public sealed class RabbitMqConsumer : BackgroundService
    {
        private readonly RabbitMqOptions _opt;
        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly ProcessedEventStore _store;
        private readonly EmailSender _email;

        private IConnection? _connection;
        private IModel? _channel;

        public RabbitMqConsumer(
            IOptions<RabbitMqOptions> opt,
            ILogger<RabbitMqConsumer> logger,
            ProcessedEventStore store,
            EmailSender email)
        {
            _opt = opt.Value;
            _logger = logger;
            _store = store;
            _email = email;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await _store.EnsureSchemaAsync(cancellationToken);

            var factory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = _opt.HostName,
                Port = _opt.Port,
                UserName = _opt.UserName,
                Password = _opt.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_opt.Exchange, ExchangeType.Topic, durable: true, autoDelete: false);

            _channel.QueueDeclare(_opt.Queue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(_opt.Queue, _opt.Exchange, _opt.RoutingKey);

            _channel.BasicQos(0, prefetchCount: 10, global: false);

            _logger.LogInformation("Worker connected. Exchange={Exchange} Queue={Queue} RoutingKey={Key}",
                _opt.Exchange, _opt.Queue, _opt.RoutingKey);

            await base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel is null) throw new InvalidOperationException("Channel not initialized.");

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (_, ea) =>
            {
                var messageId = ea.BasicProperties?.MessageId ?? string.Empty;

                try
                {
                    if (string.IsNullOrWhiteSpace(messageId))
                    {
                        _logger.LogWarning("Message received without MessageId. Rejecting.");
                        _channel.BasicReject(ea.DeliveryTag, requeue: false);
                        return;
                    }

                    // idempotencia
                    var firstTime = await _store.TryMarkProcessedAsync(messageId, stoppingToken);
                    if (!firstTime)
                    {
                        _logger.LogInformation("Duplicate message ignored. MessageId={MessageId}", messageId);
                        _channel.BasicAck(ea.DeliveryTag, multiple: false);
                        return;
                    }

                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var routingKey = ea.RoutingKey;

                    _logger.LogInformation("Event received. MessageId={MessageId} RoutingKey={RoutingKey} Body={Body}",
                        messageId, routingKey, json);

                    // enviar correo (demo)
                    await _email.SendAsync(
                        subject: $"[Demo03] Event: {routingKey}",
                        body: json,
                        ct: stoppingToken);

                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message. MessageId={MessageId}", messageId);
                    // Para demo: requeue=false para no ciclar infinito
                    _channel.BasicReject(ea.DeliveryTag, requeue: false);
                }
            };

            _channel.BasicConsume(queue: _opt.Queue, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            try { _channel?.Close(); } catch { }
            try { _connection?.Close(); } catch { }
            return base.StopAsync(cancellationToken);
        }
    }
}
