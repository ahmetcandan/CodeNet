using CodeNet.Core;
using CodeNet.EntityFramework.InMemory.Extensions;
using CodeNet.Parameters.Manager;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using CodeNet.Parameters.Extensions;
using Microsoft.Extensions.Configuration;

namespace CodeNet.MakerChecker.Tests
{
    public class ParametersTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Maker_Checker_Approve_Tests()
        {
            Mock<ICodeNetContext> mockCodeNetContext = new();
            mockCodeNetContext.Setup(c => c.UserName)
                .Returns("admin");
            mockCodeNetContext.Setup(c => c.Roles)
                .Returns(["Admin"]);

            IServiceCollection services = new ServiceCollection();
            var configurationManager = new ConfigurationManager();
            configurationManager.AddJsonFile("testSettings.json");
            services.AddParameters(options => options.UseInMemoryDatabase("TestParameterDb"), configurationManager.GetSection("Redis"), configurationManager.GetSection("Parameters"));
            services.AddScoped(c => mockCodeNetContext.Object);

            var serviceProvider = services.BuildServiceProvider();
            var parameterManager = serviceProvider.GetRequiredService<IParameterManager>();
            var parameterGroup = await parameterManager.AddParameterGroupAsync(new Parameters.Models.AddParameterGroupModel
            {
                Code = "TG",
                ApprovalRequired = false,
                Description = "Test Group"
            });
            Assert.That(parameterGroup, Is.Not.Null);
            var p1 = await parameterManager.AddParameterAsync(new Parameters.Models.AddParameterModel
            {
                Code = parameterGroup.Code + "1",
                Value = "TG1_Value",
                GroupId = parameterGroup.Id
            });
            Assert.That(p1, Is.Not.Null);
            var p2 = await parameterManager.AddParameterAsync(new Parameters.Models.AddParameterModel
            {
                Code = parameterGroup.Code + "2",
                Value = "TG2_Value",
                GroupId = parameterGroup.Id
            });
            Assert.That(p2, Is.Not.Null);

            var parameterList = await parameterManager.GetParametersAsync(parameterGroup.Id);
            Assert.Multiple(() =>
            {
                Assert.That(parameterList, Is.Not.Null);
                Assert.That(parameterList, Has.Count.EqualTo(2));
                Assert.That(true);
            });
        }
    }
}