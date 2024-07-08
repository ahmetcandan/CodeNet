﻿using CodeNet.Core;
using CodeNet.EntityFramework.InMemory.Extensions;
using CodeNet.MakerChecker.Extensions;
using CodeNet.MakerChecker.Models;
using CodeNet.MakerChecker.Tests.Mock.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
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

        //[Test]
        //public void Maker_Checker_Approve_Tests()
        //{
        //    Mock<ICodeNetContext> mockIdentityContext = new();
        //    mockIdentityContext.Setup(c => c.UserName)
        //        .Returns("admin");
        //    mockIdentityContext.Setup(c => c.Roles)
        //        .Returns(["Admin"]);
            
        //    var webBuilder = WebApplication.CreateBuilder();
        //    webBuilder.Services.AddMakerChecker<MockMakerCheckerDbContext>(c => c.UseInMemoryDatabase("TestApprovedDb"));
        //    var app = webBuilder.Build();
            
        //    var dbContext = app.Services.GetRequiredService<MockMakerCheckerDbContext>();
        //    var codeNetContext = app.Services.GetRequiredService<ICodeNetContext>();

        //    var tableRepository = new TestTableRepository(dbContext, codeNetContext);
        //    var makerCheckerManager = app.Services.GetRequiredService<IMakerCheckerManager>();

        //    var definitionId = makerCheckerManager.InsertDefinition<TestTable>();
        //    makerCheckerManager.InsertFlow(new FlowInserModel
        //    {
        //        Approver = "admin",
        //        Description = "Admin Onayı",
        //        ApproveType = ApproveType.User,
        //        DefinitionId = definitionId,
        //        Order = 1
        //    });
        //    makerCheckerManager.InsertFlow(new FlowInserModel
        //    {
        //        Approver = "admin",
        //        Description = "Admin Onayı 2",
        //        ApproveType = ApproveType.User,
        //        DefinitionId = definitionId,
        //        Order = 2
        //    });

        //    //insert test data
        //    var entity = tableRepository.Add(new TestTable
        //    {
        //        Name = "Test kaydı",
        //        Id = 1
        //    });
        //    //insert test data + insert flow history
        //    var saveChangeResponse = tableRepository.SaveChanges();
        //    Assert.That(saveChangeResponse, Is.EqualTo(3));

        //    var pendingTest = tableRepository.GetDraft(entity.ReferenceId!.Value);
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(pendingTest, Is.Not.Null);
        //        Assert.That(pendingTest!.ApproveStatus, Is.EqualTo(ApproveStatus.Pending));
        //        Assert.That(pendingTest.Id, Is.EqualTo(entity.ReferenceId));
        //    });

        //    var pendingList = makerCheckerManager.GetPendingList();
        //    Assert.That(pendingList?.Count, Is.EqualTo(2));

        //    //approve test data
        //    tableRepository.Approve(pendingTest, "test onayı");
        //    tableRepository.SaveChanges();


        //    pendingList = makerCheckerManager.GetPendingList();
        //    Assert.That(pendingList?.Count, Is.EqualTo(1));

        //    //get approved data
        //    var approvedTest = tableRepository.GetDraft(entity.ReferenceId.Value);
        //    Assert.That(approvedTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Pending));

        //    //approve test data
        //    var approvedData = tableRepository.Approve(approvedTest, "ikinci test onayı");
        //    tableRepository.SaveChanges();

        //    pendingList = makerCheckerManager.GetPendingList();
        //    Assert.That(pendingList?.Count, Is.EqualTo(0));

        //    approvedTest = tableRepository.GetDraft(entity.ReferenceId.Value);
        //    Assert.That(approvedTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Approved));

        //    //get approved data
        //    var result = tableRepository.Get(approvedData!.Id);
        //    Assert.That(result, Is.Not.Null);
        //}

        //[Test]
        //public async Task Maker_Checker_Approve_Async_Tests()
        //{
        //    Mock<ICodeNetContext> mockIdentityContext = new();
        //    mockIdentityContext.Setup(c => c.UserName)
        //        .Returns("admin");
        //    mockIdentityContext.Setup(c => c.Roles)
        //        .Returns(["Admin"]);
        //    var options = new DbContextOptionsBuilder<MakerCheckerDbContext>().UseInMemoryDatabase("TestApproveAsyncDb").Options;
        //    var dbContext = new MockMakerCheckerDbContext(options);
        //    var tableRepository = new TestTableRepository(dbContext, mockIdentityContext.Object);
        //    var makerCheckerManager = new MakerCheckerManager(dbContext, mockIdentityContext.Object);

        //    var definitionId = await makerCheckerManager.InsertDefinitionAsync<TestTable>();
        //    await makerCheckerManager.InsertFlowAsync(new FlowInserModel
        //    {
        //        Approver = "admin",
        //        Description = "Admin Onayı",
        //        ApproveType = ApproveType.User,
        //        DefinitionId = definitionId,
        //        Order = 1
        //    });
        //    await makerCheckerManager.InsertFlowAsync(new FlowInserModel
        //    {
        //        Approver = "admin",
        //        Description = "Admin Onayı 2",
        //        ApproveType = ApproveType.User,
        //        DefinitionId = definitionId,
        //        Order = 2
        //    });

        //    //insert test data
        //    var entity = await tableRepository.AddAsync(new TestTable
        //    {
        //        Name = "Test kaydı",
        //        Id = 1
        //    });
        //    //insert test data + insert flow history
        //    var saveChangeResponse = await tableRepository.SaveChangesAsync();
        //    Assert.That(saveChangeResponse, Is.EqualTo(3));

        //    var pendingTest = await tableRepository.GetDraftAsync(entity.ReferenceId!.Value);
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(pendingTest, Is.Not.Null);
        //        Assert.That(pendingTest!.ApproveStatus, Is.EqualTo(ApproveStatus.Pending));
        //        Assert.That(pendingTest.Id, Is.EqualTo(entity.ReferenceId));
        //    });

        //    var pendingList = await makerCheckerManager.GetPendingListAsync();
        //    Assert.That(pendingList?.Count, Is.EqualTo(2));

        //    //approve test data
        //    tableRepository.Approve(pendingTest, "test onayı");
        //    tableRepository.SaveChanges();


        //    pendingList = await makerCheckerManager.GetPendingListAsync();
        //    Assert.That(pendingList?.Count, Is.EqualTo(1));

        //    //get approved data
        //    var approvedTest = await tableRepository.GetDraftAsync(entity.ReferenceId.Value);
        //    Assert.That(approvedTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Pending));

        //    //approve test data
        //    var approvedData = await tableRepository.ApproveAsync(approvedTest, "ikinci test onayı");
        //    await tableRepository.SaveChangesAsync();

        //    pendingList = await makerCheckerManager.GetPendingListAsync();
        //    Assert.That(pendingList?.Count, Is.EqualTo(0));

        //    approvedTest = await tableRepository.GetDraftAsync(entity.ReferenceId.Value);
        //    Assert.That(approvedTest?.ApproveStatus, Is.EqualTo(ApproveStatus.Approved));

        //    //get approved data
        //    var result = await tableRepository.GetAsync(approvedData!.Id);
        //    Assert.That(result, Is.Not.Null);
        //}

        //[Test]
        //public void Maker_Checker_Reject_Tests()
        //{
        //    Mock<ICodeNetContext> mockIdentityContext = new();
        //    mockIdentityContext.Setup(c => c.UserName)
        //        .Returns("admin");
        //    mockIdentityContext.Setup(c => c.Roles)
        //        .Returns(["Admin"]);
        //    var options = new DbContextOptionsBuilder<MakerCheckerDbContext>().UseInMemoryDatabase("TestRejectDb").Options;
        //    var dbContext = new MockMakerCheckerDbContext(options);
        //    var tableRepository = new TestTableRepository(dbContext, mockIdentityContext.Object);
        //    var makerCheckerManager = new MakerCheckerManager(dbContext, mockIdentityContext.Object);

        //    var definitionId = makerCheckerManager.InsertDefinition<TestTable>();
        //    makerCheckerManager.InsertFlow(new FlowInserModel
        //    {
        //        Approver = "admin",
        //        Description = "Admin Onayı",
        //        ApproveType = ApproveType.User,
        //        DefinitionId = definitionId,
        //        Order = 1
        //    });
        //    makerCheckerManager.InsertFlow(new FlowInserModel
        //    {
        //        Approver = "admin",
        //        Description = "Admin Onayı 2",
        //        ApproveType = ApproveType.User,
        //        DefinitionId = definitionId,
        //        Order = 2
        //    });

        //    //insert test data
        //    var entity = tableRepository.Add(new TestTable
        //    {
        //        Name = "Test kaydı",
        //        Id = 1
        //    });
        //    //insert test data + insert flow history
        //    var saveChangeResponse = tableRepository.SaveChanges();
        //    Assert.That(saveChangeResponse, Is.EqualTo(3));

        //    var pendingTest = tableRepository.GetDraft(entity.ReferenceId!.Value);
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(pendingTest, Is.Not.Null);
        //        Assert.That(pendingTest!.ApproveStatus, Is.EqualTo(ApproveStatus.Pending));
        //        Assert.That(pendingTest.Id, Is.EqualTo(entity.ReferenceId));
        //    });

        //    var pendingList = makerCheckerManager.GetPendingList();
        //    Assert.That(pendingList?.Count, Is.EqualTo(2));

        //    //reject test data
        //    tableRepository.Reject(pendingTest, "reddedildi...");
        //    tableRepository.SaveChanges();


        //    pendingList = makerCheckerManager.GetPendingList();
        //    Assert.That(pendingList?.Count, Is.EqualTo(0));

        //    //get approved data
        //    var draft = tableRepository.GetDraft(entity.ReferenceId.Value);
        //    Assert.That(draft, Is.Not.Null);
        //    Assert.That(draft.ApproveStatus, Is.EqualTo(ApproveStatus.Rejected));
        //}

        //[Test]
        //public async Task Maker_Checker_Reject_Async_Tests()
        //{
        //    Mock<ICodeNetContext> mockIdentityContext = new();
        //    mockIdentityContext.Setup(c => c.UserName)
        //        .Returns("admin");
        //    mockIdentityContext.Setup(c => c.Roles)
        //        .Returns(["Admin"]);
        //    var options = new DbContextOptionsBuilder<MakerCheckerDbContext>().UseInMemoryDatabase("TestRejectAsyncDb").Options;
        //    var dbContext = new MockMakerCheckerDbContext(options);
        //    var tableRepository = new TestTableRepository(dbContext, mockIdentityContext.Object);
        //    var makerCheckerManager = new MakerCheckerManager(dbContext, mockIdentityContext.Object);

        //    var definitionId = await makerCheckerManager.InsertDefinitionAsync<TestTable>();
        //    await makerCheckerManager.InsertFlowAsync(new FlowInserModel
        //    {
        //        Approver = "admin",
        //        Description = "Admin Onayı",
        //        ApproveType = ApproveType.User,
        //        DefinitionId = definitionId,
        //        Order = 1
        //    });
        //    await makerCheckerManager.InsertFlowAsync(new FlowInserModel
        //    {
        //        Approver = "admin",
        //        Description = "Admin Onayı 2",
        //        ApproveType = ApproveType.User,
        //        DefinitionId = definitionId,
        //        Order = 2
        //    });

        //    //insert test data
        //    var entity = await tableRepository.AddAsync(new TestTable
        //    {
        //        Name = "Test kaydı",
        //        Id = 1
        //    });
        //    //insert test data + insert flow history
        //    var saveChangeResponse = await tableRepository.SaveChangesAsync();
        //    Assert.That(saveChangeResponse, Is.EqualTo(3));

        //    var pendingTest = await tableRepository.GetDraftAsync(entity.ReferenceId!.Value);
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(pendingTest, Is.Not.Null);
        //        Assert.That(pendingTest!.ApproveStatus, Is.EqualTo(ApproveStatus.Pending));
        //        Assert.That(pendingTest.Id, Is.EqualTo(entity.ReferenceId));
        //    });

        //    var pendingList = await makerCheckerManager.GetPendingListAsync();
        //    Assert.That(pendingList?.Count, Is.EqualTo(2));

        //    //reject test data
        //    tableRepository.Reject(pendingTest, "reddedildi...");
        //    tableRepository.SaveChanges();


        //    pendingList = await makerCheckerManager.GetPendingListAsync();
        //    Assert.That(pendingList?.Count, Is.EqualTo(0));

        //    //get approved data
        //    var draft = await tableRepository.GetDraftAsync(entity.ReferenceId.Value);
        //    Assert.That(draft, Is.Not.Null);
        //    Assert.That(draft.ApproveStatus, Is.EqualTo(ApproveStatus.Rejected));
        //}
    }
}