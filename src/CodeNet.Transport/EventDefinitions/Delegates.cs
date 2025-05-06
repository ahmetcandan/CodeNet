using CodeNet.Transport.Client;

namespace CodeNet.Transport.EventDefinitions;

public delegate void DataReceived(DataReceivedArgs e);
public delegate void ClientConnected(TransportClient client);
public delegate void ClientDisconnected(TransportClient client);
