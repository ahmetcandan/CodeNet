using NetCore.Abstraction.Model;

namespace NetCore.EntityFramework.Tests.Mock.Model
{
    public class TestTable : TracingEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
