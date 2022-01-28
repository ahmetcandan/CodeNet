using System;

namespace NetCore.ExceptionHandling
{
    public class UserLevelException : Exception
    {
        public string UserMessage { get; set; }
        public override string StackTrace => string.Empty;
        public override string Message => UserMessage;
    }
}
