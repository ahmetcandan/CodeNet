using CodeNet.Socket.Client;

namespace CodeNet.Socket.EventDefinitions;

public delegate void ClientConnected<TClient>(ClientArguments<TClient> e) where TClient : CodeNetClient;
public delegate void ClientDisconnected<TClient>(ClientArguments<TClient> e) where TClient : CodeNetClient;
public delegate void NewMessageReceived(MessageReceivingArguments e);
public delegate void ServerNewMessageReceived<TClient>(ServerMessageReceivingArguments<TClient> e) where TClient : CodeNetClient;
public delegate void ClientConnectFinish<TClient>(ClientArguments<TClient> e) where TClient : CodeNetClient;
