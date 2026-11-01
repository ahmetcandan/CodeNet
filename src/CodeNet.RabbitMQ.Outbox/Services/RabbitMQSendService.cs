using CodeNet.BackgroundJob.Manager;
using CodeNet.Outbox.Manager;
using CodeNet.Outbox.Settings;
using CodeNet.RabbitMQ.Services;
using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RedLockNet;

namespace CodeNet.RabbitMQ.Outbox.Services;

internal class RabbitMQSendService<TOutboxProducerService, TProducerService>(IServiceProvider serviceProvider, IOptions<RabbitMQProducerOptions<TProducerService>> options) : IScheduleJob
    where TOutboxProducerService : OutboxRabbitMQProducerService<TProducerService>
    where TProducerService : RabbitMQProducerService
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var outboxService = serviceScope.ServiceProvider.GetRequiredService<IOutboxService>();
        var producerService = typeof(TProducerService).Equals(typeof(RabbitMQProducerService))
            ? serviceScope.ServiceProvider.GetRequiredService<OutboxRabbitMQProducerService>()
            : serviceScope.ServiceProvider.GetRequiredService<TOutboxProducerService>();
        var waitingMessages = await outboxService.GetWaitingMessagesAsync(options.Value.Queue, cancellationToken);
        var distributedLockService = serviceScope.ServiceProvider.GetRequiredService<IDistributedLockFactory>();
        var outboxSetting = serviceScope.ServiceProvider.GetRequiredService<IOptions<OutboxSettings>>().Value;

        foreach (var message in waitingMessages)
        {
            using var lockResult = await distributedLockService.CreateLockAsync($"{outboxSetting.RedisPrefix}-{message.Id}", TimeSpan.FromMinutes(1), TimeSpan.FromMicroseconds(100), TimeSpan.FromMicroseconds(100), cancellationToken: cancellationToken);
            if (!lockResult.IsAcquired)
                continue;
            
            var publishResult = producerService.PublishQueue(new Models.PublishModel(message.Id, message.Data, message.MessageId));
            if (publishResult is not null)
                await outboxService.DeleteMessageAsync(publishResult.Id, cancellationToken);
        }
    }
}

internal class RabbitMQSendService(IServiceProvider serviceProvider, IOptions<RabbitMQProducerOptions> options)
    : RabbitMQSendService<OutboxRabbitMQProducerService<RabbitMQProducerService>, RabbitMQProducerService>(serviceProvider, Options.Create(new RabbitMQProducerOptions<RabbitMQProducerService>
    {
        Arguments = options.Value.Arguments,
        AutoDelete = options.Value.AutoDelete,
        ConnectionFactory = options.Value.ConnectionFactory,
        DeclareExchange = options.Value.DeclareExchange,
        DeclareQueue = options.Value.DeclareQueue,
        Durable = options.Value.Durable,
        Exchange = options.Value.Exchange,
        ExchangeArguments = options.Value.ExchangeArguments,
        ExchangeType = options.Value.ExchangeType,
        Exclusive = options.Value.Exclusive,
        Mandatory = options.Value.Mandatory,
        Queue = options.Value.Queue,
        QueueBind = options.Value.QueueBind,
        QueueBindArguments = options.Value.QueueBindArguments,
        RoutingKey = options.Value.RoutingKey
    }))
{
}
