using Microsoft.EntityFrameworkCore;
using CodeNet.Abstraction;
using CodeNet.EntityFramework.Tests.Mock;
using CodeNet.EntityFramework.Tests.Mock.Model;

namespace CodeNet.EntityFramework.Tests
{
    public class BaseRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task set_tracing_info()
        {
            #region Initialize
            var username = "admin";
            var cancellationToken = CancellationToken.None;
            var options = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(databaseName: "TestDB").Options;
            using var context = new MockDbContext(options);
            var mockIdentityContext = new Mock<IIdentityContext>();
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

            Assert.Pass();
        }
    }
}