using Autofac;
using CodeNet.Identity.Extensions;
using CodeNet.Identity.Module;
using CodeNet.Core.Module;
using CodeNet.Core.Extensions;
using CodeNet.EntityFramework.InMemory.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CodeNet.Identity.Manager;

namespace CodeNet.Identity.Tests
{
    [TestFixture]
    public class IdentityTests
    {
        WebApplication _app;
        string admin = "admin";
        string adminEmail = "admin@test.net";
        string adminPassword = "@dMin-123!";
        string[] adminRoles = ["admin"];
        string user = "user";
        string userEmail = "user@test.net";
        string userPassword = "Us€r-123!";
        string[] userRoles = ["user"];

        [SetUp]
        public void Setup()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("testSettings.json");
            builder.Host.UseCodeNetContainer(containerBuilder =>
            {
                containerBuilder.RegisterModule<CodeNetModule>();
                containerBuilder.RegisterModule<IdentityModule>();
            });
            builder.AddCodeNet("Application")
                   .AddAuthentication("Identity")
                   .AddIdentity(options => options.UseInMemoryDatabase("IdentityTestDb"), "Identity");
            _app = builder.Build();
        }

        [Test]
        public async Task Add_Roles()
        {
            var roleManager = _app.Services.GetRequiredService<IIdentityRoleManager>();
            foreach (var role in userRoles.Union(adminRoles))
            {
                var result = await roleManager.CreateRole(new Model.CreateRoleModel
                {
                    Name = role
                });
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsSuccessfull, Is.True);
            }
        }

        [Test]
        public async Task Add_Users()
        {
            var userManager = _app.Services.GetRequiredService<IIdentityUserManager>();
            var adminResult = await userManager.CreateUser(new Model.RegisterUserModel
            {
                Email = adminEmail,
                Password = adminPassword,
                Username = admin,
                Roles = adminRoles
            });
            Assert.That(adminResult, Is.Not.Null);
            Assert.That(adminResult.IsSuccessfull, Is.True);

            var userResult = await userManager.CreateUser(new Model.RegisterUserModel
            {
                Email = userEmail,
                Password = userPassword,
                Username = user,
                Roles = userRoles
            });
            Assert.That(userResult, Is.Not.Null);
            Assert.That(userResult.IsSuccessfull, Is.True);
        }

        [Test]
        public async Task Generate_Token()
        {
            var tokenManager = _app.Services.GetRequiredService<IIdentityTokenManager>();
            var tokenResult = await tokenManager.GenerateToken(new Model.LoginModel 
            {
                Username = admin,
                Password = adminPassword,
            });
            Assert.Multiple(() =>
            {
                Assert.That(tokenResult, Is.Not.Null);
                Assert.That(tokenResult.IsSuccessfull, Is.True);
                Assert.That(tokenResult.FromCache, Is.False);
                Assert.That(tokenResult.Data, Is.Not.Null);
                Assert.That(tokenResult.Data?.Token, Is.Not.Null);
            });
        }
    }
}