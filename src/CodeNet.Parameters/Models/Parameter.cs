using CodeNet.EntityFramework.Models;
using CodeNet.MakerChecker.Models;

namespace CodeNet.Parameters.Models;

public class Parameter : MakerCheckerEntity, ISoftDelete
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public required string Code { get; set; }
    public required string Value { get; set; }
    public bool IsDefault { get; set; }
    public int? Order { get; set; }
}
