namespace CodeNet.Email.Services;

public partial class FunctionExecuter
{
    public static string Now(string format)
    {
        return string.IsNullOrEmpty(format) ? DateTime.Now.ToString() : DateTime.Now.ToString(format);
    }

    public static string Now()
    {
        return Now("");
    }

    public static string NumberFormat(decimal value, string format)
    {
        return value.ToString(format);
    }

    public static string Length(string value)
    {
        return value.Length.ToString();
    }
}
