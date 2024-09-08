namespace CodeNet.Kafka.Models;

public delegate Task MessageReceived<TKey, TValue>(ReceivedMessageEventArgs<TKey, TValue> e);
