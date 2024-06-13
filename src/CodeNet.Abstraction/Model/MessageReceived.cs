namespace CodeNet.Abstraction.Model;

public delegate void MessageReceived<TData>(ReceivedMessageEventArgs<TData> e) where TData : class, new();
