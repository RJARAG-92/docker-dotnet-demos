using Demo03.Notifications.Worker.Options;
using Demo03.Notifications.Worker.Services;
using Demo03.Notifications.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.Configure<PostgresOptions>(builder.Configuration.GetSection("Postgres"));
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

builder.Services.AddSingleton<ProcessedEventStore>();
builder.Services.AddSingleton<EmailSender>();
builder.Services.AddHostedService<RabbitMqConsumer>();

var host = builder.Build();
host.Run();