using CodeNet.ExceptionHandling;

namespace CodeNet.Redis.Exception;

public class RedisException(string code, string message) : CodeNetException(code, message)
{
}
