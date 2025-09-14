using System.Text.RegularExpressions;

namespace CodeNet.Messaging.Builder;

public static partial class MessagingRegex
{
    public const string _loopPattern = @"\$each\(@(?<item>[A-z][A-z0-9_]*) * in  *@(?<array>[A-z][A-z0-9_]*)\)\s*\{\{(?<body>[^}]+)\}\}";
    private const string _paramPattern = @"@(?<param>\w+(\.\w+)*)";
    private const string _funcPattern = @"\$(?<function>(?!each\b)[A-Za-z][A-Za-z0-9_]*)\((?<params>.+?)\)";
    private const string _funcParamPattern = @"(?<null>null)|(?<number>\d+\.\d+|\d+)|'(?<text>.+?|)'|@(?<param>\w+(\.\w+)*)|(?<bool>true|false)";
    private const string _ifPattern = @"\$if\((?<left>@(?<param1>\w+(\.\w+)*)|(?<number1>\d+\.\d+|\d+)|'(?<text1>.+?|)'|(?<bool1>true|false)|(?<null1>null)) *(?<operator>==|!=|<|>|<=|>=) *(?<right>@(?<param2>\w+(\.\w+)*)|(?<number2>\d+\.\d+|\d+)|'(?<text2>.+?|)'|(?<bool2>true|false)|(?<null2>null))\)\s*\{\{(?<if>[^}]+)\}\}(\s*\$else\s*\{\{(?<else>[^}]+)\}\})?";

    [GeneratedRegex(_loopPattern)]
    public static partial Regex LoopRegex();

    [GeneratedRegex(_paramPattern)]
    public static partial Regex ParamRegex();

    [GeneratedRegex(_funcPattern)]
    public static partial Regex FuncRegex();

    [GeneratedRegex(_funcParamPattern)]
    public static partial Regex FuncParamRegex();

    [GeneratedRegex(_ifPattern)]
    public static partial Regex IfRegex();
}
