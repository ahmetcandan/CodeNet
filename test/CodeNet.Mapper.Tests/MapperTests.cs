using CodeNet.Mapper.Extensions;
using CodeNet.Mapper.Services;
using CodeNet.Mapper.Tests.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Mapper.Tests
{
    public class MapperTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Convert_Tests()
        {
            Person person = new()
            {
                PersonNo = 9,
                PersonName = "Ahmet",
                BirthDate = new DateTime(1991, 12, 12),
                Details = [new PersonDetail { Description = "Candan" }, new PersonDetail { Description = "Software" }],
                Note = new Note { Context = "Test" },
                Status = Status.Active,
                Department = Department.IT,
                Ids = [1, 2, 3],
                Detail = new PersonDetail { Description = "Detail 123" }
            };
            IServiceCollection services = new ServiceCollection();
            services.AddMapper(c =>
            {
                c.CreateMap<Person, Personel>();
                c.CreateMap<PersonDetail, PersonelDetail>();
            });
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetRequiredService<ICodeNetMapper>();
            var result = mapper.MapTo<Person, Personel>(person);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.PersonNo, Is.EqualTo(person.PersonNo));
                Assert.That(result.PersonName, Is.EqualTo(person.PersonName));
                Assert.That(result.BirthDate, Is.EqualTo(person.BirthDate));
                Assert.That(result.Details?.Count, Is.EqualTo(person.Details.Count));
                Assert.That(result.Details?.Any(c => c.Description == person.Details[0].Description) is true);
                Assert.That(result.Details?.Any(c => c.Description == person.Details[1].Description) is true);
                Assert.That(result.Note?.Context, Is.EqualTo(person.Note.Context));
                Assert.That(result.Detail?.Description, Is.EqualTo(person.Detail?.Description));
                Assert.That(result.Status, Is.EqualTo(person.Status));
                Assert.That((int)result.Department, Is.EqualTo((int)person.Department));
                Assert.That(result.Ids, Is.EqualTo(person.Ids));
            });
        }
    }
}