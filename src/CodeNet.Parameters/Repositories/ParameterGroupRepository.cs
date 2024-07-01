using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.Parameters.Exception;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Parameters.Repositories;

internal class ParameterGroupRepository : TracingRepository<ParameterGroup>
{
    private readonly DbSet<Parameter> _parameters;

    public ParameterGroupRepository(DbContext dbContext, ICodeNetHttpContext identityContext) : base(dbContext, identityContext)
    {
        _parameters = _dbContext.Set<Parameter>();
    }

    public async Task<bool> GetApprovalRequiredAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return (await _entities.FindAsync([groupId], cancellationToken))?.ApprovalRequired ?? throw new ParameterException("PR001", $"Not found parameter group (Id: {groupId}).");
    }

    public bool GetApprovalRequired(int groupId)
    {
        return _entities.Find(groupId)?.ApprovalRequired ?? throw new ParameterException("PR001", $"Not found parameter group (Id: {groupId}).");
    }

    public async Task<ParameterGroupWithParamsResult?> GetParameterGroupWithParams(int groupId, CancellationToken cancellationToken)
    {
        var result = (await GetParameterGroupParameter().Where(c => c.ParameterGroup.Id == groupId).GroupBy(c => c.ParameterGroup).ToListAsync(cancellationToken)).FirstOrDefault();

        if (result is not null)
            return new ParameterGroupWithParamsResult
            {
                Code = result.Key.Code,
                ApprovalRequired = result.Key.ApprovalRequired,
                Description = result.Key.Description,
                Id = result.Key.Id,
                Parameters = result.Select(c => c.Parameter.ToParameterResult()).ToList()
            };

        return null;
    }

    public async Task<ParameterGroupWithParamsResult?> GetParameterGroupWithParams(string groupCode, CancellationToken cancellationToken)
    {
        var result = (await GetParameterGroupParameter().Where(c => c.ParameterGroup.Code == groupCode).GroupBy(c => c.ParameterGroup).ToListAsync(cancellationToken)).FirstOrDefault();

        if (result is not null)
            return new ParameterGroupWithParamsResult
            {
                Code = result.Key.Code,
                ApprovalRequired = result.Key.ApprovalRequired,
                Description = result.Key.Description,
                Id = result.Key.Id,
                Parameters = result.Select(c => c.Parameter.ToParameterResult()).ToList()
            };

        return null;
    }

    private IQueryable<ParameterGroupParameter> GetParameterGroupParameter()
    {
        return from pg in _entities
               join p in _parameters on pg.Id equals p.GroupId
               where pg.IsActive && !pg.IsDeleted
                   && p.IsActive && !p.IsDeleted
               select new ParameterGroupParameter { Parameter = p, ParameterGroup = pg };
    }

    private IQueryable<ParameterListItemResult> GetParameterQuery()
    {
        return (from g in _entities
                join p in _parameters on g.Id equals p.GroupId
                where g.IsActive && !g.IsDeleted
                   && p.IsActive && !p.IsDeleted
                select new ParameterListItemResult
                {
                    Code = p.Code,
                    GroupId = g.Id,
                    Id = p.Id,
                    Value = p.Value,
                    GroupCode = g.Code,
                    ApprovalRequired = g.ApprovalRequired
                });
    }

    public List<ParameterListItemResult> GetParameters(int groupId)
    {
        return [.. GetParameterQuery().Where(c => c.GroupId == groupId)];
    }

    public List<ParameterListItemResult> GetParameters(string groupCode)
    {
        return [.. GetParameterQuery().Where(c => c.GroupCode == groupCode)];
    }

    public Task<List<ParameterListItemResult>> GetParametersAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return GetParameterQuery().Where(c => c.GroupId == groupId).ToListAsync(cancellationToken);
    }

    public Task<List<ParameterListItemResult>> GetParametersAsync(string groupCode, CancellationToken cancellationToken = default)
    {
        return GetParameterQuery().Where(c => c.GroupCode == groupCode).ToListAsync(cancellationToken);
    }
}
