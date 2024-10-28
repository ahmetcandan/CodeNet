namespace CodeNet.Email.Builder;

internal enum ParamType
{
    Parameter = 1,
    StaticValue = 2
}

internal enum BuildType
{
    Body = 1,
    Loop = 2,
    Func = 3,
    If = 4
}

internal enum Operator
{
    Equals = 1,
    NotEquals = 2,
    GreaterThan = 3,
    GreaterThanOrEqual = 4,
    LessThan = 5,
    LessThanOrEqual = 6,
}
