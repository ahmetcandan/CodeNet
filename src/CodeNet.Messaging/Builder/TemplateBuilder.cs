﻿using CodeNet.Messaging.Exception;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeNet.Messaging.Builder;

public class TemplateBuilder : ITemplateBuilder
{
    private TemplateBuilder()
    {
    }

    public static TemplateBuilder Compile(string content)
    {
        TemplateBuilder result = new()
        {
            Index = 0,
            Content = content
        };


        StringBuilder stringBuilder = new(result.Content);

        result.AddLoopBuilder(stringBuilder.ToString());
        foreach (var builder in result.LoopBuilders.Where(c => c.Type == BuildType.Loop).OrderBy(c => c.Index))
            stringBuilder.Replace(builder.Content, "");

        result.AddIfBuilder(stringBuilder.ToString());
        foreach (var builder in result.IfBuilders.Where(c => c.Type == BuildType.If).OrderBy(c => c.Index))
            stringBuilder.Replace(builder.Content, "");

        result.AddFuncBuilder(stringBuilder.ToString());
        foreach (var builder in result.FuncBuilders.Where(c => c.Type == BuildType.Func).OrderBy(c => c.Index))
            stringBuilder.Replace(builder.Content, "");

        MatchCollection paramMatches = MessagingRegex.ParamRegex().Matches(stringBuilder.ToString());
        foreach (var match in paramMatches.Where(c => c.Success))
        {
            string paramName = match.Groups["param"].Value;
            if (!result.Parameters.Any(c => c.Name == paramName))
                result.Parameters.Add(new(paramName, ParamType.Parameter));
        }

        return result;
    }

    public StringBuilder Build(object data)
    {
        StringBuilder stringBuilder = new(Content);

        foreach (var builder in Enumerable.Empty<ITemplateBuilder>().Union(LoopBuilders).Union(FuncBuilders).Union(IfBuilders).OrderBy(c => c.Index))
        {
            stringBuilder.Replace(builder.Content, builder.Build(data).ToString());
        }

        foreach (var param in Parameters)
        {
            param.SetValue(data);
            stringBuilder.Replace($"@{param.Name}", param.Value?.ToString());
        }

        return stringBuilder;
    }

    private void AddLoopBuilder(string content)
    {
        MatchCollection eachMatches = MessagingRegex.LoopRegex().Matches(content);
        foreach (var match in eachMatches.Where(c => c.Success))
        {
            string itemName = match.Groups["item"].Value;
            string rowContent = match.Groups["body"].Value;
            string arrayName = match.Groups["array"].Value;
            LoopBuilder loopBuilder = LoopBuilder.Compile(itemName, arrayName, rowContent, match.Value, match.Index);
            LoopBuilders.Add(loopBuilder);
        }
    }

    private void AddFuncBuilder(string content)
    {
        MatchCollection funcMatches = MessagingRegex.FuncRegex().Matches(content);
        foreach (var match in funcMatches.Where(c => c.Success))
        {
            FuncBuilder funcBuilder = FuncBuilder.Compile(match.Groups["function"].Value, match.Value, match.Index);
            MatchCollection paramMatches = MessagingRegex.FuncParamRegex().Matches(match.Groups["params"].Value);
            foreach (var paramMatch in paramMatches.Where(c => c.Success))
            {
                string param = paramMatch.Groups["param"].Value;
                string number = paramMatch.Groups["number"].Value;
                string text = paramMatch.Groups["text"].Value;
                string _null = paramMatch.Groups["null"].Value;
                string boolean = paramMatch.Groups["bool"].Value;
                funcBuilder.Parameters.Add(NewParamValue(param, number, text, _null, boolean));
            }

            FuncBuilders.Add(funcBuilder);
        }
    }

    private void AddIfBuilder(string content)
    {
        MatchCollection ifMatches = MessagingRegex.IfRegex().Matches(content);
        foreach (var match in ifMatches.Where(c => c.Success))
        {
            IfBuilder ifBuilder = IfBuilder.Compile(match.Value, match.Index, match.Groups["operator"].Value, match.Groups["if"].Value, match.Groups["else"].Value);
            string param1 = match.Groups["param1"].Value;
            string number1 = match.Groups["number1"].Value;
            string text1 = match.Groups["text1"].Value;
            string _null1 = match.Groups["null1"].Value;
            string boolean1 = match.Groups["bool1"].Value;
            ifBuilder.ParamLeft = NewParamValue(param1, number1, text1, _null1, boolean1);
            string param2 = match.Groups["param2"].Value;
            string number2 = match.Groups["number2"].Value;
            string text2 = match.Groups["text2"].Value;
            string _null2 = match.Groups["null2"].Value;
            string boolean2 = match.Groups["bool2"].Value;
            ifBuilder.ParamRight = NewParamValue(param2, number2, text2, _null2, boolean2);

            IfBuilders.Add(ifBuilder);
        }
    }

    private ParamValue NewParamValue(string param, string number, string text, string _null, string boolean)
    {
        if (!string.IsNullOrEmpty(param))
        {
            return new(param, ParamType.Parameter);
        }
        else if (!string.IsNullOrEmpty(number))
        {
            return new(GenerateParamName(), double.Parse(number));
        }
        else if (!string.IsNullOrEmpty(text))
        {
            return new(GenerateParamName(), text);
        }
        else if (!string.IsNullOrEmpty(_null))
        {
            return new(GenerateParamName(), null);
        }
        else if (!string.IsNullOrEmpty(boolean))
        {
            return new(GenerateParamName(), boolean == "true");
        }

        throw new MessagingException(ExceptionMessages.IncorrectValue);
    }

    private int _staticParamId = 1;
    private string GenerateParamName()
    {
        while (Parameters.Any(c => c.Name == $"STATIC_VALUE_{_staticParamId:000}"))
            _staticParamId++;

        return $"STATIC_VALUE_{_staticParamId:000}";
    }

    public string Content { get; set; } = string.Empty;
    public int Index { get; set; }
    public ICollection<ParamValue> Parameters { get; set; } = [];
    public ICollection<LoopBuilder> LoopBuilders { get; set; } = [];
    public ICollection<FuncBuilder> FuncBuilders { get; set; } = [];
    public ICollection<IfBuilder> IfBuilders { get; set; } = [];
    public BuildType Type { get; } = BuildType.Body;
}
