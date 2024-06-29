using CodeNet.Core;
using CodeNet.EntityFramework.InMemory.Extensions;
using CodeNet.Parameters.Manager;
using CodeNet.MakerChecker.Tests.Mock.Models;
using CodeNet.Parameters;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CodeNet.MakerChecker.Tests
{
    public class ParametersTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Maker_Checker_Approve_Tests()
        {
            Mock<IIdentityContext> mockIdentityContext = new();
            mockIdentityContext.Setup(c => c.UserName)
                .Returns("admin");
            mockIdentityContext.Setup(c => c.Roles)
                .Returns(["Admin"]);
            var options = new DbContextOptionsBuilder<ParametersDbContext>().UseInMemoryDatabase("TestParametersDb").Options;
            var dbContext = new MockParametersDbContext(options);
            var parameterManager = new ParameterManager(dbContext, mockIdentityContext.Object);
            var parameterGroup = parameterManager.AddParameterGroup(new Parameters.Models.AddParameterGroupModel
            {
                Code = "TG",
                ApprovalRequired = false,
                Description = "Test Group"
            });
            Assert.That(parameterGroup, Is.Not.Null);
            var p1 = parameterManager.AddParameter(new Parameters.Models.AddParameterModel
            {
                Code = parameterGroup.Code + "1",
                Value = "TG1_Value",
                GroupId = parameterGroup.Id
            });
            Assert.That(p1, Is.Not.Null);
            var p2 = parameterManager.AddParameter(new Parameters.Models.AddParameterModel
            {
                Code = parameterGroup.Code + "2",
                Value = "TG2_Value",
                GroupId = parameterGroup.Id
            });
            Assert.That(p2, Is.Not.Null);

            var parameterList = parameterManager.GetParameters(parameterGroup.Id);
            Assert.That(parameterList, Is.Not.Null);
            Assert.That(parameterList.Count, Is.EqualTo(2));
        }
    }
}