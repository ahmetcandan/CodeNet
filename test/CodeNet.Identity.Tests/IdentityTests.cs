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
        private const string _admin = "admin";
        private const string _adminEmail = "admin@test.net";
        private const string _adminPassword = "@dMin-123!";
        private readonly string[] _adminRoles = ["admin"];
        private const string _user = "user";
        private const string _userEmail = "user@test.net";
        private const string _userPassword = "Us€r-123!";
        private readonly string[] _userRoles = ["user"];

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
            foreach (var role in _userRoles.Union(_adminRoles))
            {
                var result = await roleManager.CreateRole(new Models.CreateRoleModel
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
            var adminResult = await userManager.CreateUser(new Models.RegisterUserModel
            {
                Email = _adminEmail,
                Password = _adminPassword,
                Username = _admin,
                Roles = _adminRoles
            });
            Assert.That(adminResult, Is.Not.Null);

            var userResult = await userManager.CreateUser(new Models.RegisterUserModel
            {
                Email = _userEmail,
                Password = _userPassword,
                Username = _user,
                Roles = _userRoles
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
            var tokenResult = await tokenManager.GenerateToken(new Models.LoginModel
            {
                Username = _admin,
                Password = _adminPassword,
            });
            Assert.Multiple(() =>
            {
                Assert.That(tokenResult, Is.Not.Null);
                Assert.That(tokenResult.Token, Is.Not.Null);
            });
        }
    }
}
