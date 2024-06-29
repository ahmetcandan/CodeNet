using CodeNet.Parameters;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Tests.Mock.Models;

public class MockParametersDbContext(DbContextOptions<ParametersDbContext> options) : ParametersDbContext(options)
{
}
