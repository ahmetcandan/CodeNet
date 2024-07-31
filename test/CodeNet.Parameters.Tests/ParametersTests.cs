using CodeNet.Core;
using CodeNet.EntityFramework.InMemory.Extensions;
using CodeNet.Parameters.Extensions;
using CodeNet.Parameters.Manager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CodeNet.Parameters.Tests
{
    public class ParametersTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Parameter_Tests()
        {
            Mock<ICodeNetContext> mockCodeNetContext = new();
            mockCodeNetContext.Setup(c => c.UserName)
                .Returns("admin");
            mockCodeNetContext.Setup(c => c.Roles)
                .Returns(["Admin"]);

            IServiceCollection services = new ServiceCollection();
            var configurationManager = new ConfigurationManager();
            configurationManager.AddJsonFile("testSettings.json");
            services.AddParameters(options => options.AddDbContext(c => c.UseInMemoryDatabase("TestParameterDb")));
            services.AddScoped(c => mockCodeNetContext.Object);

            var serviceProvider = services.BuildServiceProvider();
            var parameterManager = serviceProvider.GetRequiredService<IParameterManager>();
            var p1 = new Models.ParameterModel
            {
                Code = "TG1",
                Value = "TG1_Value",
                Order = 1,
                IsDefault = true
            };
            var p2 = new Models.ParameterModel
            {
                Code = "TG2",
                Value = "TG2_Value",
                Order = 2
            };
            var p3 = new Models.ParameterModel
            {
                Code = "TG3",
                Value = "TG3_Value",
                Order = 3
            };
            var request = new Models.ParameterGroupWithParamsModel
            {
                Code = "TG",
                ApprovalRequired = false,
                Description = "Test Group",
                Parameters = [p1, p2]
            };
            var addResponse = await parameterManager.AddParameterAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That(addResponse, Is.Not.Null);
                Assert.That(addResponse.Parameters, Is.Not.Null);
                Assert.That(addResponse.Parameters.ToList(), Has.Count.EqualTo(2));
            });

            var getResponse = await parameterManager.GetParameterAsync(addResponse.Code);

            var r1 = getResponse?.Parameters.FirstOrDefault(c => c.Code == p1.Code);
            var r2 = getResponse?.Parameters.FirstOrDefault(c => c.Code == p2.Code);
            if (r1 is not null)
                p1.Id = r1.Id;
            if (r2 is not null)
                p2.Id = r2.Id;


            Assert.Multiple(() =>
            {
                Assert.That(getResponse, Is.Not.Null);
                Assert.That(getResponse?.Parameters, Is.Not.Null);
                Assert.That(getResponse?.Parameters.ToList(), Has.Count.EqualTo(2));
            });

            request.Parameters.Add(p3);
            request.Parameters.Remove(p2);
            request.Id = addResponse.Id;
            var updateResponse = await parameterManager.UpdateParameterAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That(updateResponse, Is.Not.Null);
                Assert.That(updateResponse?.Parameters, Is.Not.Null);
                Assert.That(updateResponse?.Parameters.ToList(), Has.Count.EqualTo(2));
                Assert.That(updateResponse?.Parameters.Any(c => c.Code == p1.Code), Is.True);
                Assert.That(updateResponse?.Parameters.Any(c => c.Code == p3.Code), Is.True);
                Assert.That(updateResponse?.Parameters.Any(c => c.Code == p2.Code), Is.False);
            });

            var getResponse2 = await parameterManager.GetParameterAsync(addResponse.Code);

            Assert.Multiple(() =>
            {
                Assert.That(getResponse2, Is.Not.Null);
                Assert.That(getResponse2?.Parameters, Is.Not.Null);
                Assert.That(getResponse2?.Parameters.ToList(), Has.Count.EqualTo(2));
                Assert.That(getResponse2?.Parameters.Any(c => c.Code == p1.Code), Is.True);
                Assert.That(getResponse2?.Parameters.Any(c => c.Code == p3.Code), Is.True);
                Assert.That(getResponse2?.Parameters.Any(c => c.Code == p2.Code), Is.False);
            });

        }
    }
}