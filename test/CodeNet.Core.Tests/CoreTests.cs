using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace CodeNet.Core.Tests
{
    public class CoreTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Core_Test()
        {
            var person = new Person()
            {
                Id = 1,
                Name = "Ahmet",
            };
            var json = JsonConvert.SerializeObject(person);
            var jDoc = JsonDocument.Parse(json);
            var jObj = JObject.Parse(json);
            Assert.Pass();
        }
    }

    class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}