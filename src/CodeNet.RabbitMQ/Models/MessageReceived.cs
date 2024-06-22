namespace CodeNet.RabbitMQ.Models;

public delegate void MessageReceived<TData>(ReceivedMessageEventArgs<TData> e) where TData : class, new();
