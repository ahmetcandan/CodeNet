using CodeNet.Core;
using CodeNet.EntityFramework.InMemory.Extensions;
using CodeNet.MakerChecker.Models;
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
        public void Maker_Checker_Approve_Tests()
        {
            Mock<IIdentityContext> mockIdentityContext = new();
            mockIdentityContext.Setup(c => c.UserName)
                .Returns("admin");
            mockIdentityContext.Setup(c => c.Roles)
                .Returns(["Admin"]);
            var options = new DbContextOptionsBuilder<MakerCheckerDbContext>().UseInMemoryDatabase("TestApproveDb").Options;
            var dbContext = new MockMakerCheckerDbContext(options);
            var tableRepository = new TestTableRepository(dbContext, mockIdentityContext.Object);
            var makerCheckerManager = new MakerCheckerManager(dbContext, mockIdentityContext.Object);

            var definitionId = makerCheckerManager.InsertDefinition<TestTable>();
            makerCheckerManager.InsertFlow(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı",
                ApproveType = ApproveType.User,
                DefinitionId = definitionId,
                Order = 1
            });
            makerCheckerManager.InsertFlow(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı 2",
                ApproveType = ApproveType.User,
                DefinitionId = definitionId,
                Order = 2
            });

            //insert test data
            var testEntity = tableRepository.Add(new TestTable
            {
                Name = "Test kaydı",
                Id = 1
            });
            //insert test data + insert flow history
            var saveChangeResponse = tableRepository.SaveChanges();
            Assert.That(saveChangeResponse, Is.EqualTo(3));

            var pendingTest = tableRepository.GetByReferenceId(testEntity.ReferenceId, ApproveStatus.Pending);
            Assert.Multiple(() =>
            {
                Assert.That(pendingTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Pending));
                Assert.That(pendingTest?.Id, Is.EqualTo(testEntity.Id));
            });

            var pendingList = makerCheckerManager.GetPendingList();
            Assert.That(pendingList?.Count, Is.EqualTo(1));

            //approve test data
            tableRepository.Approve(testEntity);
            tableRepository.SaveChanges();


            pendingList = makerCheckerManager.GetPendingList();
            Assert.That(pendingList?.Count, Is.EqualTo(1));

            //get approved data
            var approvedTest = tableRepository.Get(testEntity.Id);
            Assert.That(approvedTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Pending));

            //approve test data
            tableRepository.Approve(testEntity);
            tableRepository.SaveChanges();

            pendingList = makerCheckerManager.GetPendingList();
            Assert.That(pendingList?.Count, Is.EqualTo(0));

            //get approved data
            approvedTest = tableRepository.Get(testEntity.Id);
            Assert.That(approvedTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Approved));
        }

        [Test]
        public async Task Maker_Checker_Approve_Async_Tests()
        {
            Mock<IIdentityContext> mockIdentityContext = new();
            mockIdentityContext.Setup(c => c.UserName)
                .Returns("admin");
            mockIdentityContext.Setup(c => c.Roles)
                .Returns(["Admin"]);
            var options = new DbContextOptionsBuilder<MakerCheckerDbContext>().UseInMemoryDatabase("TestApproveDbAsync").Options;
            var dbContext = new MockMakerCheckerDbContext(options);
            var tableRepository = new TestTableRepository(dbContext, mockIdentityContext.Object);
            var makerCheckerManager = new MakerCheckerManager(dbContext, mockIdentityContext.Object);

            var definitionId = await makerCheckerManager.InsertDefinitionAsync<TestTable>();
            await makerCheckerManager.InsertFlowAsync(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı",
                ApproveType = ApproveType.User,
                DefinitionId = definitionId,
                Order = 1
            });
            await makerCheckerManager.InsertFlowAsync(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı 2",
                ApproveType = ApproveType.User,
                DefinitionId = definitionId,
                Order = 2
            });

            //insert test data
            var testEntity = await tableRepository.AddAsync(new TestTable
            {
                Name = "Test kaydı",
                Id = 1
            });
            //insert test data + insert flow history
            var saveChangeResponse = await tableRepository.SaveChangesAsync();
            Assert.That(saveChangeResponse, Is.EqualTo(3));

            var pendingTest = await tableRepository.GetByReferenceIdAsync(testEntity.ReferenceId, ApproveStatus.Pending);
            Assert.Multiple(() =>
            {
                Assert.That(pendingTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Pending));
                Assert.That(pendingTest?.Id, Is.EqualTo(testEntity.Id));
            });

            var pendingList = await makerCheckerManager.GetPendingListAsync();
            Assert.That(pendingList?.Count, Is.EqualTo(1));

            //approve test data
            await tableRepository.ApproveAsync(testEntity);
            await tableRepository.SaveChangesAsync();


            pendingList = await makerCheckerManager.GetPendingListAsync();
            Assert.That(pendingList?.Count, Is.EqualTo(1));

            //get approved data
            var approvedTest = await tableRepository.GetAsync(testEntity.Id);
            Assert.That(approvedTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Pending));

            //approve test data
            await tableRepository.ApproveAsync(testEntity);
            await tableRepository.SaveChangesAsync();

            pendingList = await makerCheckerManager.GetPendingListAsync();
            Assert.That(pendingList?.Count, Is.EqualTo(0));

            //get approved data
            approvedTest = await tableRepository.GetAsync(testEntity.Id);
            Assert.That(approvedTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Approved));
        }

        [Test]
        public void Maker_Checker_Reject_Tests()
        {
            Mock<IIdentityContext> mockIdentityContext = new();
            mockIdentityContext.Setup(c => c.UserName)
                .Returns("admin");
            mockIdentityContext.Setup(c => c.Roles)
                .Returns(["Admin"]);
            var options = new DbContextOptionsBuilder<MakerCheckerDbContext>().UseInMemoryDatabase("TestRejectDb").Options;
            var dbContext = new MockMakerCheckerDbContext(options);
            var tableRepository = new TestTableRepository(dbContext, mockIdentityContext.Object);
            var makerCheckerManager = new MakerCheckerManager(dbContext, mockIdentityContext.Object);

            var definitionId = makerCheckerManager.InsertDefinition<TestTable>();
            makerCheckerManager.InsertFlow(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı",
                ApproveType = ApproveType.User,
                DefinitionId = definitionId,
                Order = 1
            });
            makerCheckerManager.InsertFlow(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı 2",
                ApproveType = ApproveType.User,
                DefinitionId = definitionId,
                Order = 2
            });

            //insert test data
            var testEntity = tableRepository.Add(new TestTable
            {
                Name = "Test kaydı",
                Id = 1
            });
            //insert test data + insert flow history
            var saveChangeResponse = tableRepository.SaveChanges();
            Assert.That(saveChangeResponse, Is.EqualTo(3));

            var pendingTest = tableRepository.GetByReferenceId(testEntity.ReferenceId, ApproveStatus.Pending);
            Assert.Multiple(() =>
            {
                Assert.That(pendingTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Pending));
                Assert.That(pendingTest?.Id, Is.EqualTo(testEntity.Id));
            });

            var pendingList = makerCheckerManager.GetPendingList();
            Assert.That(pendingList?.Count, Is.EqualTo(1));

            //approve test data
            tableRepository.Reject(testEntity);
            tableRepository.SaveChanges();


            pendingList = makerCheckerManager.GetPendingList();
            Assert.That(pendingList?.Count, Is.EqualTo(0));

            //get approved data
            var approvedTest = tableRepository.Get(testEntity.Id);
            Assert.That(approvedTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Rejected));
        }

        [Test]
        public async Task Maker_Checker_Reject_Async_Tests()
        {
            Mock<IIdentityContext> mockIdentityContext = new();
            mockIdentityContext.Setup(c => c.UserName)
                .Returns("admin");
            mockIdentityContext.Setup(c => c.Roles)
                .Returns(["Admin"]);
            var options = new DbContextOptionsBuilder<MakerCheckerDbContext>().UseInMemoryDatabase("TestRejecAsynctDb").Options;
            var dbContext = new MockMakerCheckerDbContext(options);
            var tableRepository = new TestTableRepository(dbContext, mockIdentityContext.Object);
            var makerCheckerManager = new MakerCheckerManager(dbContext, mockIdentityContext.Object);

            var definitionId = await makerCheckerManager.InsertDefinitionAsync<TestTable>();
            await makerCheckerManager.InsertFlowAsync(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı",
                ApproveType = ApproveType.User,
                DefinitionId = definitionId,
                Order = 1
            });
            await makerCheckerManager.InsertFlowAsync(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı 2",
                ApproveType = ApproveType.User,
                DefinitionId = definitionId,
                Order = 2
            });

            //insert test data
            var testEntity = await tableRepository.AddAsync(new TestTable
            {
                Name = "Test kaydı",
                Id = 1
            });
            //insert test data + insert flow history
            var saveChangeResponse = await tableRepository.SaveChangesAsync();
            Assert.That(saveChangeResponse, Is.EqualTo(3));

            var pendingTest = await tableRepository.GetByReferenceIdAsync(testEntity.ReferenceId, ApproveStatus.Pending);
            Assert.Multiple(() =>
            {
                Assert.That(pendingTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Pending));
                Assert.That(pendingTest?.Id, Is.EqualTo(testEntity.Id));
            });

            var pendingList = makerCheckerManager.GetPendingList();
            Assert.That(pendingList?.Count, Is.EqualTo(1));

            //approve test data
            await tableRepository.RejectAsync(testEntity);
            await tableRepository.SaveChangesAsync();


            pendingList = await makerCheckerManager.GetPendingListAsync();
            Assert.That(pendingList?.Count, Is.EqualTo(0));

            //get approved data
            var approvedTest = await tableRepository.GetAsync(testEntity.Id);
            Assert.That(approvedTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Rejected));
        }
    }
}