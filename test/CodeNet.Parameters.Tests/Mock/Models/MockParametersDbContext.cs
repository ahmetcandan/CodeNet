using CodeNet.Parameters.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Parameters.Tests.Mock.Models;

public class MockParametersDbContext(DbContextOptions<ParametersDbContext> options) : ParametersDbContext(options)
{
}
