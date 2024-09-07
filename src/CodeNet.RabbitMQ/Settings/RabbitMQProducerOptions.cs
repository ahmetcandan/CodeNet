﻿using CodeNet.RabbitMQ.Services;

namespace CodeNet.RabbitMQ.Settings;

public class RabbitMQProducerOptions : BaseRabbitMQOptions
{
    public bool? Mandatory { get; set; } = false;

    public override string ToString()
    {
        return $"{base.ToString()} Mandatory: {Mandatory}";
    }
}

public class RabbitMQProducerOptions<TProducerService> : RabbitMQProducerOptions
    where TProducerService : RabbitMQProducerService
{
    public override string ToString()
    {
        return base.ToString();
    }
}
