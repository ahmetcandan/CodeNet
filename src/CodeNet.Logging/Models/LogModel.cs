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

    private string DataToString()
    {
        if (Data is null)
            return "null";

        if (isJsonData)
            return Data;

        return $@"""{Data}""";
    }

    /// <summary>
    /// Total Miliseconds
    /// </summary>
    public virtual long? ElapsedDuration { get; set; }

    public override string ToString()
    {
        return @$"{{
    ""CorrelationId"": ""{CorrelationId}"",
    ""AssemblyName"": ""{AssemblyName}"",
    ""ClassName"": ""{ClassName}"",
    ""MethodName"": ""{MethodName}"",
    ""LogTime"": ""{LogTime}"",
    ""ElapsedDuration"": {ElapsedDuration?.ToString() ?? "null"},
    ""Data"": {DataToString()},
    ""Username"": {(Username is not null ? $@"""{Username}""" : "null")}
}}";
    }
}
