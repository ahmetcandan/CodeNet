using CodeNet.Core.Context;
using CodeNet.EntityFramework.InMemory.Extensions;
using CodeNet.MakerChecker.Extensions;
using CodeNet.MakerChecker.Models;
using CodeNet.MakerChecker.Service;
using CodeNet.MakerChecker.Tests.Mock.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            Mock<ICodeNetContext> mockCodeNetContext = new();
            mockCodeNetContext.Setup(c => c.UserName)
                .Returns("admin");
            mockCodeNetContext.Setup(c => c.Roles)
                .Returns(["Admin"]);

            IServiceCollection services = new ServiceCollection();
            var configurationManager = new ConfigurationManager();
            services.AddMakerChecker<MockMakerCheckerDbContext>(options => options.UseInMemoryDatabase("TestMCDb"));
            services.AddScoped(c => mockCodeNetContext.Object);
            var serviceProvider = services.BuildServiceProvider();
            var makerCheckerManager = serviceProvider.GetRequiredService<IMakerCheckerManager>();
            var dbContext = serviceProvider.GetRequiredService<MockMakerCheckerDbContext>();
            var tableRepository = new TestTableRepository(dbContext, mockCodeNetContext.Object);

            makerCheckerManager.InsertFlow<TestTable>(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı",
                ApproveType = ApproveType.User,
                Order = 1
            });
            makerCheckerManager.InsertFlow<TestTable>(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı 2",
                ApproveType = ApproveType.User,
                Order = 2
            });

            //insert test data
            var entity = tableRepository.Add(new TestTable
            {
                Name = "Test kaydı"
            });
            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.ReferenceId, Is.Not.Empty);

            //insert test data + insert flow history
            var saveChangeResponse = tableRepository.SaveChanges();
            Assert.Multiple(() =>
            {
                Assert.That(entity.Id, Is.EqualTo(1));
                Assert.That(saveChangeResponse, Is.EqualTo(3));
            });
            var mainId = entity.Id;

            var pendingList = makerCheckerManager.GetPendingList();
            Assert.That(pendingList, Has.Count.EqualTo(2));

            var pendingItem = pendingList.First(c => c.Flow.Order == entity.Order);
            Assert.That(pendingItem, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(pendingItem, Is.Not.Null);
                Assert.That(pendingItem?.ReferenceId, Is.EqualTo(entity.ReferenceId));
            });

            //approve test data
            var firstApprove = makerCheckerManager.Approve<TestTable>(pendingItem.ReferenceId, "test onayı");
            Assert.That(firstApprove, Is.Not.Null);
            Assert.That(firstApprove.EntityStatus, Is.EqualTo(EntityStatus.Pending));

            entity = tableRepository.GetByReferenceId(entity.ReferenceId);
            Assert.That(entity, Is.Not.Null);


            pendingList = makerCheckerManager.GetPendingList();
            Assert.That(pendingList?.Count, Is.EqualTo(1));

            pendingItem = pendingList.First(c => c.Flow.Order == entity.Order);
            Assert.That(pendingItem, Is.Not.Null);

            //approve test data
            var lastApprove = makerCheckerManager.Approve<TestTable>(pendingItem.ReferenceId, "ikinci test onayı");
            Assert.That(lastApprove, Is.Not.Null);
            Assert.That(lastApprove.EntityStatus, Is.EqualTo(EntityStatus.Completed));

            pendingList = makerCheckerManager.GetPendingList();
            Assert.That(pendingList?.Count, Is.EqualTo(0));

            //get approved data
            entity = tableRepository.Get(c => c.Id == mainId);
            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.EntityStatus, Is.EqualTo(EntityStatus.Completed));

            string updateName = "Test kaydı 2";
            entity.Name = updateName;
            var tempEntity = tableRepository.Update(entity);
            dbContext.SaveChanges();
            Assert.That(tempEntity, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(tempEntity.Id, Is.EqualTo(2));
                Assert.That(tempEntity.EntityStatus, Is.EqualTo(EntityStatus.Pending));
                Assert.That(tempEntity.MainReferenceId, Is.Not.Null);
            });

            var firstUpdateApprove = makerCheckerManager.Approve<TestTable>(tempEntity.ReferenceId, "test update onayı");
            Assert.That(firstUpdateApprove, Is.Not.Null);
            Assert.That(firstUpdateApprove.EntityStatus, Is.EqualTo(EntityStatus.Pending));

            var lastUpdateApprove = makerCheckerManager.Approve<TestTable>(tempEntity.ReferenceId, "ikinci test update onayı");
            Assert.That(lastUpdateApprove, Is.Not.Null);
            Assert.That(lastUpdateApprove.EntityStatus, Is.EqualTo(EntityStatus.Completed));

            entity = tableRepository.Get(c => c.Id == mainId);
            Assert.That(entity, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(entity.Name, Is.EqualTo(updateName));
                Assert.That(entity.EntityStatus, Is.EqualTo(EntityStatus.Completed));
            });

            var deletedTempEntity = tableRepository.Remove(entity);
            dbContext.SaveChanges();
            Assert.That(deletedTempEntity, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(deletedTempEntity.Id, Is.EqualTo(3));
                Assert.That(deletedTempEntity.EntityStatus, Is.EqualTo(EntityStatus.Pending));
            });

            var firstDeleteApprove = makerCheckerManager.Approve<TestTable>(deletedTempEntity.ReferenceId, "test delete onayı");
            Assert.That(firstDeleteApprove, Is.Not.Null);
            Assert.That(firstDeleteApprove.EntityStatus, Is.EqualTo(EntityStatus.Pending));

            var lastDeleteApprove = makerCheckerManager.Approve<TestTable>(deletedTempEntity.ReferenceId, "ikinci test delete onayı");
            Assert.That(lastDeleteApprove, Is.Not.Null);
            Assert.That(lastDeleteApprove.EntityStatus, Is.EqualTo(EntityStatus.Completed));

            entity = tableRepository.Get(c => c.Id == mainId);
            Assert.That(entity, Is.Null);
        }

        [Test]
        public async Task Maker_Checker_Approve_Async_Tests()
        {
            Mock<ICodeNetContext> mockCodeNetContext = new();
            mockCodeNetContext.Setup(c => c.UserName)
                .Returns("admin");
            mockCodeNetContext.Setup(c => c.Roles)
                .Returns(["Admin"]);

            IServiceCollection services = new ServiceCollection();
            var configurationManager = new ConfigurationManager();
            services.AddMakerChecker<MockMakerCheckerDbContext>(options => options.UseInMemoryDatabase("TestAsyncMCDb"));
            services.AddScoped(c => mockCodeNetContext.Object);
            var serviceProvider = services.BuildServiceProvider();
            var makerCheckerManager = serviceProvider.GetRequiredService<IMakerCheckerManager>();
            var dbContext = serviceProvider.GetRequiredService<MockMakerCheckerDbContext>();
            var tableRepository = new TestTableRepository(dbContext, mockCodeNetContext.Object);

            await makerCheckerManager.InsertFlowAsync<TestTable>(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı",
                ApproveType = ApproveType.User,
                Order = 1
            });
            await makerCheckerManager.InsertFlowAsync<TestTable>(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı 2",
                ApproveType = ApproveType.User,
                Order = 2
            });

            //insert test data
            var entity = await tableRepository.AddAsync(new TestTable
            {
                Name = "Test kaydı"
            });
            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.ReferenceId, Is.Not.Empty);

            //insert test data + insert flow history
            var saveChangeResponse = await tableRepository.SaveChangesAsync();
            Assert.Multiple(() =>
            {
                Assert.That(entity.Id, Is.EqualTo(1));
                Assert.That(saveChangeResponse, Is.EqualTo(3));
            });
            var mainId = entity.Id;

            var pendingList = await makerCheckerManager.GetPendingListAsync();
            Assert.That(pendingList, Has.Count.EqualTo(2));

            var pendingItem = pendingList.First(c => c.Flow.Order == entity.Order);
            Assert.That(pendingItem, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(pendingItem, Is.Not.Null);
                Assert.That(pendingItem?.ReferenceId, Is.EqualTo(entity.ReferenceId));
            });

            //approve test data
            var firstApprove = await makerCheckerManager.ApproveAsync<TestTable>(pendingItem.ReferenceId, "test onayı");
            Assert.That(firstApprove, Is.Not.Null);
            Assert.That(firstApprove.EntityStatus, Is.EqualTo(EntityStatus.Pending));

            entity = await tableRepository.GetByReferenceIdAsync(entity.ReferenceId);
            Assert.That(entity, Is.Not.Null);


            pendingList = await makerCheckerManager.GetPendingListAsync();
            Assert.That(pendingList?.Count, Is.EqualTo(1));

            pendingItem = pendingList.First(c => c.Flow.Order == entity.Order);
            Assert.That(pendingItem, Is.Not.Null);

            //approve test data
            var lastApprove = await makerCheckerManager.ApproveAsync<TestTable>(pendingItem.ReferenceId, "ikinci test onayı");
            Assert.That(lastApprove, Is.Not.Null);
            Assert.That(lastApprove.EntityStatus, Is.EqualTo(EntityStatus.Completed));

            pendingList = await makerCheckerManager.GetPendingListAsync();
            Assert.That(pendingList?.Count, Is.EqualTo(0));

            //get approved data
            entity = await tableRepository.GetAsync(c => c.Id == mainId);
            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.EntityStatus, Is.EqualTo(EntityStatus.Completed));

            string updateName = "Test kaydı 2";
            entity.Name = updateName;
            var tempEntity = await tableRepository.UpdateAsync(entity);
            await dbContext.SaveChangesAsync();
            Assert.That(tempEntity, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(tempEntity.Id, Is.EqualTo(2));
                Assert.That(tempEntity.EntityStatus, Is.EqualTo(EntityStatus.Pending));
                Assert.That(tempEntity.MainReferenceId, Is.Not.Null);
            });

            var firstUpdateApprove = await makerCheckerManager.ApproveAsync<TestTable>(tempEntity.ReferenceId, "test update onayı");
            Assert.That(firstUpdateApprove, Is.Not.Null);
            Assert.That(firstUpdateApprove.EntityStatus, Is.EqualTo(EntityStatus.Pending));

            var lastUpdateApprove = await makerCheckerManager.ApproveAsync<TestTable>(tempEntity.ReferenceId, "ikinci test update onayı");
            Assert.That(lastUpdateApprove, Is.Not.Null);
            Assert.That(lastUpdateApprove.EntityStatus, Is.EqualTo(EntityStatus.Completed));

            entity = await tableRepository.GetAsync(c => c.Id == mainId);
            Assert.That(entity, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(entity.Name, Is.EqualTo(updateName));
                Assert.That(entity.EntityStatus, Is.EqualTo(EntityStatus.Completed));
            });

            var deletedTempEntity = await tableRepository.RemoveAsync(entity);
            await dbContext.SaveChangesAsync();
            Assert.That(deletedTempEntity, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(deletedTempEntity.Id, Is.EqualTo(3));
                Assert.That(deletedTempEntity.EntityStatus, Is.EqualTo(EntityStatus.Pending));
            });

            var firstDeleteApprove = await makerCheckerManager.ApproveAsync<TestTable>(deletedTempEntity.ReferenceId, "test delete onayı");
            Assert.That(firstDeleteApprove, Is.Not.Null);
            Assert.That(firstDeleteApprove.EntityStatus, Is.EqualTo(EntityStatus.Pending));

            var lastDeleteApprove = await makerCheckerManager.ApproveAsync<TestTable>(deletedTempEntity.ReferenceId, "ikinci test delete onayı");
            Assert.That(lastDeleteApprove, Is.Not.Null);
            Assert.That(lastDeleteApprove.EntityStatus, Is.EqualTo(EntityStatus.Completed));

            entity = await tableRepository.GetAsync(c => c.Id == mainId);
            Assert.That(entity, Is.Null);
        }

        [Test]
        public void Maker_Checker_Reject_Tests()
        {

            Mock<ICodeNetContext> mockCodeNetContext = new();
            mockCodeNetContext.Setup(c => c.UserName)
                .Returns("admin");
            mockCodeNetContext.Setup(c => c.Roles)
                .Returns(["Admin"]);

            IServiceCollection services = new ServiceCollection();
            var configurationManager = new ConfigurationManager();
            services.AddMakerChecker<MockMakerCheckerDbContext>(options => options.UseInMemoryDatabase("TestRMCDb"));
            services.AddScoped(c => mockCodeNetContext.Object);
            var serviceProvider = services.BuildServiceProvider();
            var makerCheckerManager = serviceProvider.GetRequiredService<IMakerCheckerManager>();
            var dbContext = serviceProvider.GetRequiredService<MockMakerCheckerDbContext>();
            var tableRepository = new TestTableRepository(dbContext, mockCodeNetContext.Object);

            makerCheckerManager.InsertFlow<TestTable>(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı",
                ApproveType = ApproveType.User,
                Order = 1
            });
            makerCheckerManager.InsertFlow<TestTable>(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı 2",
                ApproveType = ApproveType.User,
                Order = 2
            });

            //insert test data
            var entity = tableRepository.Add(new TestTable
            {
                Name = "Test kaydı",
                Id = 1
            });
            //insert test data + insert flow history
            var saveChangeResponse = tableRepository.SaveChanges();
            Assert.That(saveChangeResponse, Is.EqualTo(3));

            var pendingTest = tableRepository.GetByReferenceId(entity.ReferenceId);
            Assert.Multiple(() =>
            {
                Assert.That(pendingTest, Is.Not.Null);
                Assert.That(pendingTest!.EntityStatus, Is.EqualTo(EntityStatus.Pending));
                Assert.That(pendingTest.ReferenceId, Is.EqualTo(entity.ReferenceId));
            });

            var pendingList = makerCheckerManager.GetPendingList();
            Assert.That(pendingList?.Count, Is.EqualTo(2));

            var pendingItem = pendingList.OrderBy(c => c.Flow.Order).First();

            //reject test data
            makerCheckerManager.Reject<TestTable>(pendingItem.ReferenceId, "reddedildi...");


            pendingList = makerCheckerManager.GetPendingList();
            Assert.That(pendingList?.Count, Is.EqualTo(0));

            //get approved data
            var resultEntity = tableRepository.Get(c => c.Id == entity.Id);
            Assert.That(resultEntity, Is.Null);
        }

        [Test]
        public async Task Maker_Checker_Reject_Async_Tests()
        {
            Mock<ICodeNetContext> mockCodeNetContext = new();
            mockCodeNetContext.Setup(c => c.UserName)
                .Returns("admin");
            mockCodeNetContext.Setup(c => c.Roles)
                .Returns(["Admin"]);

            IServiceCollection services = new ServiceCollection();
            var configurationManager = new ConfigurationManager();
            services.AddMakerChecker<MockMakerCheckerDbContext>(options => options.UseInMemoryDatabase("TestAsyncRMCDb"));
            services.AddScoped(c => mockCodeNetContext.Object);
            var serviceProvider = services.BuildServiceProvider();
            var makerCheckerManager = serviceProvider.GetRequiredService<IMakerCheckerManager>();
            var dbContext = serviceProvider.GetRequiredService<MockMakerCheckerDbContext>();
            var tableRepository = new TestTableRepository(dbContext, mockCodeNetContext.Object);

            await makerCheckerManager.InsertFlowAsync<TestTable>(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı",
                ApproveType = ApproveType.User,
                Order = 1
            });
            await makerCheckerManager.InsertFlowAsync<TestTable>(new FlowInserModel
            {
                Approver = "admin",
                Description = "Admin Onayı 2",
                ApproveType = ApproveType.User,
                Order = 2
            });

            //insert test data
            var entity = await tableRepository.AddAsync(new TestTable
            {
                Name = "Test kaydı",
                Id = 1
            });
            //insert test data + insert flow history
            var saveChangeResponse = await tableRepository.SaveChangesAsync();
            Assert.That(saveChangeResponse, Is.EqualTo(3));

            var pendingTest = await tableRepository.GetByReferenceIdAsync(entity.ReferenceId);
            Assert.Multiple(() =>
            {
                Assert.That(pendingTest, Is.Not.Null);
                Assert.That(pendingTest!.EntityStatus, Is.EqualTo(EntityStatus.Pending));
                Assert.That(pendingTest.ReferenceId, Is.EqualTo(entity.ReferenceId));
            });

            var pendingList = await makerCheckerManager.GetPendingListAsync();
            Assert.That(pendingList?.Count, Is.EqualTo(2));

            var pendingItem = pendingList.OrderBy(c => c.Flow.Order).First();

            //reject test data
            await makerCheckerManager.RejectAsync<TestTable>(pendingItem.ReferenceId, "reddedildi...");


            pendingList = await makerCheckerManager.GetPendingListAsync();
            Assert.That(pendingList?.Count, Is.EqualTo(0));

            //get approved data
            var resultEntity = await tableRepository.GetAsync(c => c.Id == entity.Id);
            Assert.That(resultEntity, Is.Null);
        }
    }
}