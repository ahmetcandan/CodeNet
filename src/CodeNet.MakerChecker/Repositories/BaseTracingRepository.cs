using CodeNet.Core;
using CodeNet.EntityFramework.Models;
using CodeNet.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Repositories;

internal class BaseTracingRepository<TEntity>(DbContext dbContext, ICodeNetContext identityContext) : TracingRepository<TEntity>(dbContext, identityContext)
    where TEntity : class, ITracingEntity
{
}
