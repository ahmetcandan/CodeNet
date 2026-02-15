using CodeNet.Core.Enums;
using CodeNet.Core.Extensions;
using Newtonsoft.Json;
using System.Text;

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
                Now = new DateTime(2024, 7, 19)
            };
            dynamic pDynamic = person;
            var xSeriliaze = JsonConvert.SerializeObject(pDynamic);
            var json = JsonConvert.SerializeObject(person);

            var x = new StringContent(xSeriliaze, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json);
            var str = x.ReadAsStringAsync().Result;
            Assert.That(str, Is.EqualTo(json));
            Assert.Pass();
        }

        [Test]
        public void Enum_Test()
        {
            CacheStates noCache = CacheStates.NoCache;
            CacheStates clearCache = CacheStates.ClearCache;

            var noCacheName = noCache.GetDisplayName();
            var clearCacheName = clearCache.GetDisplayName();

            Assert.Multiple(() =>
            {
                Assert.That(noCacheName, Is.Not.Empty);
                Assert.That(clearCacheName, Is.Not.Empty);
            });
            Assert.Pass();
        }
    }

    internal class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Now { get; set; }
    }
}