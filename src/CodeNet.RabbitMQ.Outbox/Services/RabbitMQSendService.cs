using CodeNet.BackgroundJob.Manager;
using CodeNet.Outbox.Manager;
using CodeNet.RabbitMQ.Services;
using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        var result = producerService.PublishRange(waitingMessages.Select(c => new Models.PublishModel(c.Id, c.Data, c.MessageId)));
        await outboxService.DeleteMessageAsync(result.Select(c => c.Id), cancellationToken);
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
