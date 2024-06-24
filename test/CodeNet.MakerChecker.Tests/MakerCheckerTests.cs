using CodeNet.Core;
using CodeNet.EntityFramework.InMemory.Extensions;
using CodeNet.MakerChecker.Repositories;
using CodeNet.MakerChecker.Tests.Mock.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CodeNet.MakerChecker.Tests
{
    public class MakerCheckerTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var options = new DbContextOptionsBuilder<MakerCheckerDbContext>().UseInMemoryDatabase("TestMakerCheckerDb").Options;
            var dbContext = new MockMakerCheckerDbContext(options);
            Mock<IIdentityContext> mockIdentityContext = new();
            mockIdentityContext.Setup(c => c.UserName)
                .Returns("admin");
            mockIdentityContext.Setup(c => c.Roles)
                .Returns(["Admin"]);


            var testRepository = new TestTableRepository(dbContext, mockIdentityContext.Object);
            MakerCheckerManager makerCheckerManager = new(dbContext, mockIdentityContext.Object);
            var definitionId = makerCheckerManager.InsertDefinition(new Models.DefinitionInserModel
            {
                EntityName = nameof(TestTable)
            });
            makerCheckerManager.InsertFlow(new Models.FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı",
                ApproveType = Models.ApproveType.User,
                DefinitionId = definitionId,
                Order = 1
            });
            var test = testRepository.Add(new TestTable
            {
                Name = "Test kaydı",
                Id = 1
            });
            testRepository.SaveChanges();

            var pendingTest = testRepository.FindByStatus(c => c.ReferenceId == test.ReferenceId, Models.ApproveStatus.Pending);

            testRepository.Approve(test);

            var approvedTest = testRepository.Get(test.Id);

            Assert.Pass();
        }
    }
}