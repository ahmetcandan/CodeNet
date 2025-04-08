using CodeNet.Core.Enums;
using CodeNet.Core.Extensions;
using CodeNet.EntityFramework.InMemory.Extensions;
using CodeNet.Identity.Extensions;
using CodeNet.Identity.Manager;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Identity.Tests
{
    [TestFixture]
    public class IdentityTests
    {
        private readonly string admin = "admin";
        private readonly string adminEmail = "admin@test.net";
        private readonly string adminPassword = "@dMin-123!";
        private readonly string[] adminRoles = ["admin"];
        private readonly string user = "user";
        private readonly string userEmail = "user@test.net";
        private readonly string userPassword = "Us€r-123!";
        private readonly string[] userRoles = ["user"];

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Add_Roles()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("testSettings.json");
            builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"))
                   .AddAuthorization(options => options.UseInMemoryDatabase("IdentityTestDb"), SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("Identity"));
            new CodeNetOptionsBuilder(builder.Services).AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("Identity"));
            var _app = builder.Build();
            var roleManager = _app.Services.GetRequiredService<IIdentityRoleManager>();
            foreach (var role in userRoles.Union(adminRoles))
            {
                var result = await roleManager.CreateRole(new Settings.CreateRoleModel
                {
                    Name = role
                });
                Assert.That(result, Is.Not.Null);
            }
        }

        [Test]
        public async Task Add_Users()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("testSettings.json");
            builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"))
                   .AddAuthorization(options => options.UseInMemoryDatabase("IdentityTestDb"), SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("Identity"));
            new CodeNetOptionsBuilder(builder.Services).AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("Identity"));
            var _app = builder.Build();
            var userManager = _app.Services.GetRequiredService<IIdentityUserManager>();
            var adminResult = await userManager.CreateUser(new Settings.RegisterUserModel
            {
                Email = adminEmail,
                Password = adminPassword,
                Username = admin,
                Roles = adminRoles
            });
            Assert.That(adminResult, Is.Not.Null);

            var userResult = await userManager.CreateUser(new Settings.RegisterUserModel
            {
                Email = userEmail,
                Password = userPassword,
                Username = user,
                Roles = userRoles
            });
            Assert.That(userResult, Is.Not.Null);
        }

        [Test]
        public async Task Generate_Token()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("testSettings.json");
            builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"))
                   .AddAuthorization(options => options.UseInMemoryDatabase("IdentityTestDb"), SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("Identity"));
            new CodeNetOptionsBuilder(builder.Services).AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("Identity"));
            var _app = builder.Build();
            var tokenManager = _app.Services.GetRequiredService<IIdentityTokenManager>();
            var tokenResult = await tokenManager.GenerateToken(new Settings.LoginModel
            {
                Username = admin,
                Password = adminPassword,
            });
            Assert.Multiple(() =>
            {
                Assert.That(tokenResult, Is.Not.Null);
                Assert.That(tokenResult.Token, Is.Not.Null);
            });
        }
    }
}
