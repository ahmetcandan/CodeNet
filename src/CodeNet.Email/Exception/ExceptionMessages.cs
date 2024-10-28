using CodeNet.Core.Models;

namespace CodeNet.Email.Exception
{
    internal static class ExceptionMessages
    {
        public static ExceptionMessage LoopItemParam { get { return new ExceptionMessage("EM0001", "Loop item name not equals param name!"); } }
        public static ExceptionMessage IncorrectOperator { get { return new ExceptionMessage("EM0002", "Incorrect operator usage!"); } }
        public static ExceptionMessage IncorrectValue { get { return new ExceptionMessage("EM0003", "Incorrect parameter value!"); } }
        public static ExceptionMessage IfConditionException { get { return new ExceptionMessage("EM0004", "If condition exception!"); } }

        public static ExceptionMessage UseParams(this ExceptionMessage exceptionMessage, params string[] args) => new(exceptionMessage.Code, string.Format(exceptionMessage.Message, args));
    }
}
