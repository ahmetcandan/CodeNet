using CodeNet.Core.Models;

namespace CodeNet.Identity.Exception
{
    internal static class ExceptionMessages
    {
        public static ExceptionMessage UserNotFound { get { return new ExceptionMessage("ID0001", "User not found!"); } }
        public static ExceptionMessage InvalidToken { get { return new ExceptionMessage("ID0002", "Invalid token."); } }
        public static ExceptionMessage InvalidRefreshToken { get { return new ExceptionMessage("ID0003", "Invalid refresh token."); } }
        public static ExceptionMessage IncorrectUserPass { get { return new ExceptionMessage("ID0004", "Username or password incorrect."); } }
        public static ExceptionMessage RefreshTokenExpired { get { return new ExceptionMessage("ID0005", "Refresh token has expired."); } }
        public static ExceptionMessage UserAlreadyExists { get { return new ExceptionMessage("ID0006", "User already exists."); } }
        public static ExceptionMessage UserCreationFailed { get { return new ExceptionMessage("ID0007", "User creation failed! Please check user details and try again."); } }
        public static ExceptionMessage RoleAlreadyExists { get { return new ExceptionMessage("ID0008", "Role already exists."); } }
        public static ExceptionMessage RoleCreationFailed { get { return new ExceptionMessage("ID0009", "Role creation failed! Please check role details and try again."); } }
        public static ExceptionMessage RoleUpdateFailed { get { return new ExceptionMessage("ID0010", "Role update failed! Please check role details and try again."); } }
        public static ExceptionMessage RoleDeleteFailed { get { return new ExceptionMessage("ID0010", "Role delete failed! Please check role details and try again."); } }
        public static ExceptionMessage RoleNotFound { get { return new ExceptionMessage("ID0012", "Role not found!"); } }
        public static ExceptionMessage RefreshTokenRemoveFailed { get { return new ExceptionMessage("ID0013", "Refresh token remove failed."); } }

        public static ExceptionMessage UseParams(this ExceptionMessage exceptionMessage, params string[] args) => new(exceptionMessage.Code, string.Format(exceptionMessage.Message, args));
    }
}
