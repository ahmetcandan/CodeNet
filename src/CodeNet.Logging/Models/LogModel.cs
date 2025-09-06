namespace CodeNet.Logging.Models;

public class LogModel(bool isJsonData)
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime Date { get; } = DateTime.Now;
    public virtual string CorrelationId { get; set; } = string.Empty;
    public virtual string? Username { get; set; }
    public virtual string AssemblyName { get; set; } = string.Empty;
    public virtual string ClassName { get; set; } = string.Empty;
    public virtual string MethodName { get; set; } = string.Empty;
    public virtual string LogTime { get; set; } = string.Empty;
    public virtual string? Data { get; set; }

    /// <summary>
    /// Total Miliseconds
    /// </summary>
    public virtual long? ElapsedDuration { get; set; }

    public override string ToString() => ToJson(isJsonData, Data, ElapsedDuration, CorrelationId, AssemblyName, ClassName, MethodName, LogTime, Username);

    internal static string ToJson(bool isJsonData, string? data, long? elapsedDuration, params string?[] args)
    {
        return @$"{{
    ""CorrelationId"": ""{args[0]}"",
    ""AssemblyName"": ""{args[1]}"",
    ""ClassName"": ""{args[2]}"",
    ""MethodName"": ""{args[3]}"",
    ""LogTime"": ""{args[4]}"",
    ""ElapsedDuration"": {elapsedDuration?.ToString() ?? "null"},
    ""Data"": {(data is null ? "null" : isJsonData ? data : $@"""{data}""")},
    ""Username"": {(args[5] is not null ? $@"""{args[5]}""" : "null")}
}}";
    }
}
