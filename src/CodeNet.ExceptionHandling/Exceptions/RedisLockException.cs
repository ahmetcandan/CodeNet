namespace CodeNet.ExceptionHandling.Exceptions;

public class RedisLockException(string code, string message) : CodeNetException(code, message)
{
}
