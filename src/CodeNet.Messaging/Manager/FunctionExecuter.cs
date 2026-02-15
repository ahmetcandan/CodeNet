using System.Globalization;

namespace CodeNet.Messaging.Manager;

public partial class FunctionExecuter
{
    private FunctionExecuter() { }

    public static string Now() => Now("");

    public static string Now(string format) => string.IsNullOrEmpty(format) ? DateTime.Now.ToString() : DateTime.Now.ToString(format, CultureInfo.InvariantCulture);

    public static string NumberFormat(decimal value, string format) => value.ToString(format, CultureInfo.InvariantCulture);

    public static string NumberFormat(int value, string format) => value.ToString(format, CultureInfo.InvariantCulture);

    public static string NumberFormat(double value, string format) => value.ToString(format, CultureInfo.InvariantCulture);

    public static string DateFormat(DateTime date, string format) => date.ToString(format, CultureInfo.InvariantCulture);

    public static string Length(string value) => value.Length.ToString();
}
