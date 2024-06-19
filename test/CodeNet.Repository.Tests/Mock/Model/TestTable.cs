using CodeNet.EntityFramework.Models;

namespace CodeNet.EntityFramework.Tests.Mock.Model
{
    public class TestTable : TracingEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
