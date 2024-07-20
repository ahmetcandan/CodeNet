using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.Parameters.Exception;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
        return (await _entities.FindAsync([groupId], cancellationToken))?.ApprovalRequired ?? throw new ParameterException("PR001", $"Not found parameter group (Id: {groupId}).");
    }

    public bool GetApprovalRequired(int groupId)
    {
        return _entities.Find(groupId)?.ApprovalRequired ?? throw new ParameterException("PR001", $"Not found parameter group (Id: {groupId}).");
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
        var result = (await GetParameterGroupParameter().Where(c => c.ParameterGroup.Code == groupCode).ToListAsync(cancellationToken))
            .GroupBy(c => new
            {
                c.ParameterGroup.Id,
                c.ParameterGroup.Code,
                c.ParameterGroup.ApprovalRequired,
                c.ParameterGroup.Description
            }).FirstOrDefault();

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
        return (from pg in _entities
                join p in _parameters on pg.Id equals p.GroupId
                where pg.IsActive && !pg.IsDeleted
                    && p.IsActive && !p.IsDeleted
                select new ParameterGroupParameter { Parameter = p, ParameterGroup = pg })
                .AsNoTracking();
    }
}
