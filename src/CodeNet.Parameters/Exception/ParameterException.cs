using CodeNet.ExceptionHandling;

namespace CodeNet.Parameters.Exception;

public class ParameterException(string code, string message) : CodeNetException(code, message)
{
}
