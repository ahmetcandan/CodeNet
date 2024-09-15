using CodeNet.Core.Models;

namespace CodeNet.MakerChecker.Exception
{
    internal static class ExceptionMessages
    {
        public static ExceptionMessage EntityNotFound { get { return new ExceptionMessage("MC0001", "Entity not found!"); } }
        public static ExceptionMessage NoPendingData { get { return new ExceptionMessage("MC0002", "This is an not pending data."); } }
        public static ExceptionMessage NoPendingFlow { get { return new ExceptionMessage("MC0003", "There are no pending flow."); } }
        public static ExceptionMessage Unauthorized { get { return new ExceptionMessage("MC0004", "Unauthorized user/role."); } }
        public static ExceptionMessage Rejected { get { return new ExceptionMessage("MC0005", "This registration was previously rejected."); } }
        public static ExceptionMessage FlowNotFound { get { return new ExceptionMessage("MC0006", "MakerCheckerFlow not found."); } }
        public static ExceptionMessage MethodNotUsed { get { return new ExceptionMessage("MC0007", "'{0}' method is not used for this 'MakerCheckerRepository'."); } }

        public static ExceptionMessage UseParams(this ExceptionMessage exceptionMessage, params string[] args) => new(exceptionMessage.Code, string.Format(exceptionMessage.Message, args));
    }
}
