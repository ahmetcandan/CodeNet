using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Tests.Mock.Models;

[EntityName("Test")]
public class TestTable : MakerCheckerEntity
{
    [PrimaryKey]
    public int Id { get; set; }
    public required string Name { get; set; }
}
