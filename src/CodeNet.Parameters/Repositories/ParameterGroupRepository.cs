using CodeNet.Core.Context;
using CodeNet.EntityFramework.Repositories;
using CodeNet.Parameters.Exception;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Parameters.Repositories;

internal class ParameterGroupRepository : TracingRepository<ParameterGroup>
{
    private readonly DbSet<Parameter> _parameters;

    public ParameterGroupRepository(DbContext dbContext, ICodeNetContext identityContext) : base(dbContext, identityContext)
    {
        _parameters = _dbContext.Set<Parameter>();
    }

    public ValueTask<ParameterGroup?> GetParameterGroupAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return _entities.FindAsync([groupId], cancellationToken);
    }

    public async Task<bool> GetApprovalRequiredAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return (await _entities.FindAsync([groupId], cancellationToken))?.ApprovalRequired ?? throw new ParameterException(ExceptionMessages.NotFoundGroup.UseParams(groupId.ToString()));
    }

    public bool GetApprovalRequired(int groupId)
    {
        return _entities.Find(groupId)?.ApprovalRequired ?? throw new ParameterException(ExceptionMessages.NotFoundGroup.UseParams(groupId.ToString()));
    }

    public async Task<ParameterGroupWithParamsResult?> GetParameterGroupWithParams(int groupId, CancellationToken cancellationToken)
    {
        var result = (await GetParameterGroupParameter().Where(c => c.ParameterGroup.Id == groupId).ToListAsync(cancellationToken))
            .GroupBy(c => new
            {
                c.ParameterGroup.Id,
                c.ParameterGroup.Code,
                c.ParameterGroup.ApprovalRequired,
                c.ParameterGroup.Description
            }).FirstOrDefault();

        return result is not null
            ? new ParameterGroupWithParamsResult
            {
                Code = result.Key.Code,
                ApprovalRequired = result.Key.ApprovalRequired,
                Description = result.Key.Description,
                Id = result.Key.Id,
                Parameters = result.Select(c => c.Parameter.ToParameterResult())
            }
            : null;
    }

    public async Task<ParameterGroupWithParamsResult?> GetParameterGroupWithParams(string groupCode, CancellationToken cancellationToken)
    {
        var result = (await GetParameterGroupParameter().Where(c => c.ParameterGroup.Code == groupCode).ToListAsync(cancellationToken))
            .GroupBy(c => new
            {
                c.ParameterGroup.Id,
                c.ParameterGroup.Code,
                c.ParameterGroup.ApprovalRequired,
                c.ParameterGroup.Description
            }).FirstOrDefault();

        return result is not null
            ? new ParameterGroupWithParamsResult
            {
                Code = result.Key.Code,
                ApprovalRequired = result.Key.ApprovalRequired,
                Description = result.Key.Description,
                Id = result.Key.Id,
                Parameters = result.Select(c => c.Parameter.ToParameterResult())
            }
            : null;
    }

    public async Task<ParameterGroupWithDefaultParamResult?> GetParameterGroupWithDefaultParam(string groupCode, CancellationToken cancellationToken)
    {
        var result = (await GetParameterGroupParameter(true).Where(c => c.ParameterGroup.Code == groupCode).ToListAsync(cancellationToken))
            .GroupBy(c => new
            {
                c.ParameterGroup.Id,
                c.ParameterGroup.Code,
                c.ParameterGroup.ApprovalRequired,
                c.ParameterGroup.Description
            }).FirstOrDefault();

        return result is not null
            ? new ParameterGroupWithDefaultParamResult
            {
                Code = result.Key.Code,
                ApprovalRequired = result.Key.ApprovalRequired,
                Description = result.Key.Description,
                Id = result.Key.Id,
                Parameter = result.Select(c => c.Parameter.ToParameterResult()).FirstOrDefault()
            }
            : null;
    }

    private IQueryable<ParameterGroupParameter> GetParameterGroupParameter(bool hasIsDefault = false)
    {
        return (from pg in _entities
                join p in _parameters.Where(c => !hasIsDefault || c.IsDefault).OrderBy(c => c.Order) on pg.Id equals p.GroupId into pi
                from p in pi.DefaultIfEmpty()
                where pg.IsActive && !pg.IsDeleted
                    && p.IsActive && !p.IsDeleted
                select new ParameterGroupParameter { Parameter = p, ParameterGroup = pg })
                .AsNoTracking();
    }
}
