using CodeNet.EventBus.Client;

namespace CodeNet.EventBus.Server;

internal class CodeNetEventBusServer(int port) : CodeNetServer<CodeNetEventBusClient>(port)
{
}
