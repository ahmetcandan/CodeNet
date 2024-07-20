using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
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
            DateTime dt = DateTime.Now;
            var person = new Person()
            {
                Id = 1,
                Name = "Ahmet",
                Now = new DateTime(2024, 7, 19)
            };
            dynamic pDynamic = person;
            var xSeriliaze = JsonConvert.SerializeObject(pDynamic);
            var json = JsonConvert.SerializeObject(person);

            var jDoc = JsonDocument.Parse(json);
            var jObj = JObject.Parse(json);
            var x = new StringContent(xSeriliaze, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json);
            var str = x.ReadAsStringAsync().Result;
            Assert.Pass();
        }
    }

    internal class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Now { get; set; }
    }
}