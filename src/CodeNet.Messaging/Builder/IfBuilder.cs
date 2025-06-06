using CodeNet.Messaging.Exception;
using System.Text;

namespace CodeNet.Messaging.Builder;

public class IfBuilder : ITemplateBuilder
{
    private IfBuilder()
    {
    }

    public static IfBuilder Compile(string content, int index, string operation, string ifContent, string? elseContent = null)
    {
        var _operator = operation switch
        {
            "==" => Operator.Equals,
            "!=" => Operator.NotEquals,
            ">" => Operator.GreaterThan,
            ">=" => Operator.GreaterThanOrEqual,
            "<" => Operator.LessThan,
            "<=" => Operator.LessThanOrEqual,
            _ => throw new MessagingException(ExceptionMessages.LoopItemParam)
        };
        IfBuilder builder = new()
        {
            Content = content,
            Index = index,
            Operator = _operator,
            IfBodyBuilder = TemplateBuilder.Compile(ifContent),
            ElseBodyBuilder = string.IsNullOrEmpty(elseContent) ? null : TemplateBuilder.Compile(elseContent)
        };

        return builder;
    }

    private static bool ParamEquals(ParamValue param1, ParamValue param2) => (!param1.HasValue && !param2.HasValue) || (param1.HasValue && param2.HasValue && param1.Value!.Equals(param2.Value));

    public StringBuilder Build(object data)
    {
        if (ParamLeft.Type == ParamType.Parameter)
            ParamLeft.SetValue(data);

        if (ParamRight.Type == ParamType.Parameter)
            ParamRight.SetValue(data);

        bool? result = Operator switch
        {
            Operator.Equals => ParamEquals(ParamLeft, ParamRight),
            Operator.NotEquals => !ParamEquals(ParamLeft, ParamRight),
            _ => null
        };

        Type[] types = [typeof(int), typeof(decimal), typeof(double)];
        if (result is null && types.Contains(ParamLeft.Value!.GetType()) && types.Contains(ParamRight.Value!.GetType()))
            result = Operator switch
            {
                Operator.GreaterThan => (decimal)ParamLeft.Value > (decimal)ParamRight.Value,
                Operator.GreaterThanOrEqual => (decimal)ParamLeft.Value >= (decimal)ParamRight.Value,
                Operator.LessThan => (decimal)ParamLeft.Value < (decimal)ParamRight.Value,
                Operator.LessThanOrEqual => (decimal)ParamLeft.Value <= (decimal)ParamRight.Value,
                _ => null
            };

        return result is null
            ? throw new MessagingException(ExceptionMessages.IfConditionException)
            : result.Value ? IfBodyBuilder.Build(data) : (ElseBodyBuilder?.Build(data) ?? new(""));
    }

    public string Content { get; set; } = string.Empty;
    public int Index { get; set; }
    public ParamValue ParamLeft { get; set; }
    public ParamValue ParamRight { get; set; }
    public required TemplateBuilder IfBodyBuilder { get; set; }
    public TemplateBuilder? ElseBodyBuilder { get; set; }
    public Operator Operator { get; set; }
    public BuildType Type { get; } = BuildType.If;
}
