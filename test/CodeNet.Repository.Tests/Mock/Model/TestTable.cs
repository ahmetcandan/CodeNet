using CodeNet.EntityFramework.Models;

namespace CodeNet.EntityFramework.Tests.Mock.Model
{
    public class TestTable : TracingEntity, ISoftDelete
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
