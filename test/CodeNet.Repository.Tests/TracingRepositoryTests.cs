using Microsoft.EntityFrameworkCore;
using CodeNet.EntityFramework.Tests.Mock;
using CodeNet.EntityFramework.Tests.Mock.Model;
using CodeNet.Core;

namespace CodeNet.EntityFramework.Tests
{
    public class TracingRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Set_Tracing_Info_Tests()
        {
            #region Initialize
            var username = "admin";
            var options = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(databaseName: "TestDB").Options;
            using var context = new MockDbContext(options);
            var mockIdentityContext = new Mock<ICodeNetContext>();
            mockIdentityContext.Setup(c => c.UserName)
                .Returns(username);
            var testRepository = new TestTableRepository(context, mockIdentityContext.Object);
            var data1 = new TestTable
            {
                Name = "Ahmet"
            };
            #endregion

            #region Insert
            var now = DateTime.Now;
            var insertedData = testRepository.Add(data1);
            testRepository.SaveChanges();

            Assert2.AreEqualByJson(insertedData, data1);
            Assert.Multiple(() =>
            {
                Assert.That(insertedData.IsActive, Is.True);
                Assert.That(insertedData.IsDeleted, Is.False);
                Assert.That(insertedData.CreatedUser, Is.EqualTo(username));
                Assert.That(insertedData.CreatedDate, Is.EqualTo(now).Within(TimeSpan.FromSeconds(1)));
                Assert.That(insertedData.ModifiedUser, Is.Null);
                Assert.That(insertedData.ModifiedDate, Is.Null);
            });
            #endregion

            #region Get
            var foundData = testRepository.Get(insertedData.Id);
            Assert.That(foundData, Is.Not.Null);
            Assert2.AreEqualByJson(foundData, insertedData);
            #endregion

            #region Modify
            now = DateTime.Now;
            var updatedData = testRepository.Update(foundData);
            _ = testRepository.SaveChanges();
            Assert2.AreEqualByJson(updatedData, foundData);
            Assert.Multiple(() =>
            {
                Assert.That(updatedData.ModifiedUser, Is.EqualTo(username));
                Assert.That(updatedData.ModifiedDate, Is.EqualTo(now).Within(TimeSpan.FromSeconds(1)));
            });
            #endregion

            #region Delete
            now = DateTime.Now;
            var deletedData = testRepository.Remove(updatedData);
            _ = testRepository.SaveChanges();
            Assert2.AreEqualByJson(deletedData, updatedData);
            Assert.Multiple(() =>
            {
                Assert.That(deletedData.ModifiedDate, Is.EqualTo(now).Within(TimeSpan.FromSeconds(1)));
                Assert.That(insertedData.IsDeleted, Is.True);
            });
            #endregion
        }

        [Test]
        public async Task Set_Tracing_Info_Async_Tests()
        {
            #region Initialize
            var username = "admin";
            var cancellationToken = CancellationToken.None;
            var options = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(databaseName: "TestAsyncDB").Options;
            using var context = new MockDbContext(options);
            var mockIdentityContext = new Mock<ICodeNetContext>();
            mockIdentityContext.Setup(c => c.UserName)
                .Returns(username);
            var testRepository = new TestTableRepository(context, mockIdentityContext.Object);
            var data1 = new TestTable
            {
                Name = "Ahmet"
            };
            #endregion

            #region Insert
            var now = DateTime.Now;
            var insertedData = await testRepository.AddAsync(data1, cancellationToken);
            await testRepository.SaveChangesAsync(cancellationToken);

            Assert2.AreEqualByJson(insertedData, data1);
            Assert.Multiple(() =>
            {
                Assert.That(insertedData.IsActive, Is.True);
                Assert.That(insertedData.IsDeleted, Is.False);
                Assert.That(insertedData.CreatedUser, Is.EqualTo(username));
                Assert.That(insertedData.CreatedDate, Is.EqualTo(now).Within(TimeSpan.FromSeconds(1)));
                Assert.That(insertedData.ModifiedUser, Is.Null);
                Assert.That(insertedData.ModifiedDate, Is.Null);
            });
            #endregion

            #region Get
            var foundData = await testRepository.GetAsync([insertedData.Id], cancellationToken);
            Assert.That(foundData, Is.Not.Null);
            Assert2.AreEqualByJson(foundData, insertedData);
            #endregion

            #region Modify
            now = DateTime.Now;
            var updatedData = testRepository.Update(foundData);
            _ = await testRepository.SaveChangesAsync(cancellationToken);
            Assert2.AreEqualByJson(updatedData, foundData);
            Assert.Multiple(() =>
            {
                Assert.That(updatedData.ModifiedUser, Is.EqualTo(username));
                Assert.That(updatedData.ModifiedDate, Is.EqualTo(now).Within(TimeSpan.FromSeconds(1)));
            });
            #endregion

            #region Delete
            now = DateTime.Now;
            var deletedData = testRepository.Remove(updatedData);
            _ = await testRepository.SaveChangesAsync(cancellationToken);
            Assert2.AreEqualByJson(deletedData, updatedData);
            Assert.Multiple(() =>
            {
                Assert.That(deletedData.ModifiedDate, Is.EqualTo(now).Within(TimeSpan.FromSeconds(1)));
                Assert.That(insertedData.IsDeleted, Is.True);
            });
            #endregion
        }

        [Test]
        public void Add_Update_Range_Tests()
        {
            #region Initialize
            var username = "admin";
            var options = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(databaseName: "Test2DB").Options;
            using var context = new MockDbContext(options);
            var mockIdentityContext = new Mock<ICodeNetContext>();
            mockIdentityContext.Setup(c => c.UserName)
                .Returns(username);
            var testRepository = new TestTableRepository(context, mockIdentityContext.Object);
            var data1 = new TestTable
            {
                Name = "Ahmet"
            }; 
            var data2 = new TestTable
            {
                Name = "İrem"
            };
            var data3 = new TestTable
            {
                Name = "Ela"
            };
            #endregion

            #region AddRange

            var list = new List<TestTable> { data1, data2, data3 };
            testRepository.AddRange(list);
            var result = testRepository.SaveChanges();
            Assert.That(result, Is.EqualTo(3));
            var resultList = testRepository.Find(c => c.IsActive);
            Assert.That(resultList, Is.Not.Null);
            Assert.That(resultList, Has.Count.EqualTo(3));
            Assert.That(resultList.First().CreatedUser, Is.EqualTo(username));

            #endregion

            #region UpdateRange

            foreach (var item in resultList)
            {
                item.Name += "_";
            }
            var updatedList = testRepository.UpdateRange(resultList).ToList();
            Assert.That(updatedList, Is.Not.Null);
            Assert.That(updatedList, Has.Count.EqualTo(3));
            Assert.That(resultList.First().ModifiedUser, Is.EqualTo(username));

            #endregion
        }

        [Test]
        public async Task Add_Update_Range_Async_Tests()
        {
            #region Initialize
            var username = "admin";
            var options = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(databaseName: "Test2AsyncDB").Options;
            using var context = new MockDbContext(options);
            var mockIdentityContext = new Mock<ICodeNetContext>();
            mockIdentityContext.Setup(c => c.UserName)
                .Returns(username);
            var testRepository = new TestTableRepository(context, mockIdentityContext.Object);
            var data1 = new TestTable
            {
                Name = "Ahmet"
            };
            var data2 = new TestTable
            {
                Name = "İrem"
            };
            var data3 = new TestTable
            {
                Name = "Ela"
            };
            #endregion

            #region AddRange

            var list = new List<TestTable> { data1, data2, data3 };
            await testRepository.AddRangeAsync(list);
            var result = await testRepository.SaveChangesAsync();
            Assert.That(result, Is.EqualTo(3));
            var resultList = await testRepository.FindAsync(c => c.IsActive);
            Assert.That(resultList, Is.Not.Null);
            Assert.That(resultList, Has.Count.EqualTo(3));
            Assert.That(resultList.First().CreatedUser, Is.EqualTo(username));

            #endregion

            #region UpdateRange

            foreach (var item in resultList)
            {
                item.Name += "_";
            }
            var updatedList = testRepository.UpdateRange(resultList).ToList();
            Assert.That(updatedList, Is.Not.Null);
            Assert.That(updatedList, Has.Count.EqualTo(3));
            Assert.That(resultList.First().ModifiedUser, Is.EqualTo(username));

            #endregion
        }
    }
}