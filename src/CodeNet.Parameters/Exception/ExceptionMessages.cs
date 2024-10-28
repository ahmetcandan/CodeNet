using CodeNet.Core.Models;

namespace CodeNet.Parameters.Exception
{
    internal static class ExceptionMessages
    {
        public static ExceptionMessage NotFoundGroup { get { return new ExceptionMessage("PR001", "Not found parameter group (Id: {0})."); } }

        public static ExceptionMessage UseParams(this ExceptionMessage exceptionMessage, params string[] args) => new(exceptionMessage.Code, string.Format(exceptionMessage.Message, args));
    }
}
