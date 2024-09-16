using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Tests.Mock.Models;

public class TestTable : MakerCheckerEntity
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }
    public required string Name { get; set; }
}
