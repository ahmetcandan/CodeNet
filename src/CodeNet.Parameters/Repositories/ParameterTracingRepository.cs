using CodeNet.Core.Context;
using CodeNet.EntityFramework.Repositories;
using CodeNet.Parameters.Models;

namespace CodeNet.Parameters.Repositories;

internal class ParameterTracingRepository(Microsoft.EntityFrameworkCore.DbContext dbContext, ICodeNetContext identityContext) : TracingRepository<Parameter>(dbContext, identityContext), IParameterRepository
{
}
