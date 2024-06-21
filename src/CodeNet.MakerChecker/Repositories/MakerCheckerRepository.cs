using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Repositories;

public class MakerCheckerRepository<TMakerCheckerEntity> : TracingRepository<TMakerCheckerEntity>, IMakerCheckerRepository<TMakerCheckerEntity>
    where TMakerCheckerEntity : class, IMakerCheckerEntity
{
    private readonly DbSet<MakerCheckerDefinition> _makerCheckerDefinitions;
    private readonly DbSet<MakerCheckerFlow> _makerCheckerFlows;
    private readonly string _entityName = typeof(TMakerCheckerEntity).Name;
    private readonly IMakerCheckerHistoryRepository _makerCheckerHistoryRepository;

    public MakerCheckerRepository(MakerCheckerDbContext dbContext, IIdentityContext identityContext, IMakerCheckerHistoryRepository MakerCheckerHistoryRepository) : base(dbContext, identityContext)
    {
        _makerCheckerDefinitions = _dbContext.Set<MakerCheckerDefinition>();
        _makerCheckerFlows = _dbContext.Set<MakerCheckerFlow>();
        _makerCheckerHistoryRepository = MakerCheckerHistoryRepository;
    }

    public override TMakerCheckerEntity Add(TMakerCheckerEntity entity)
    {
        var makerCheckerFlows = GetMakerCheckerFlows();

        MakerCheckerRepository<TMakerCheckerEntity>.EntityResetStatus(entity);
        foreach (var item in makerCheckerFlows)
            _makerCheckerHistoryRepository.Add(NewMakerCheckerHistory(item, entity.ReferenceId));

        return base.Add(entity);
    }

    public override Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity)
    {
        return AddAsync(entity, CancellationToken.None);
    }

    public override async Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken)
    {
        var makerCheckerFlows = await GetAsyncMakerCheckerFlows(cancellationToken);

        MakerCheckerRepository<TMakerCheckerEntity>.EntityResetStatus(entity);
        foreach (var item in makerCheckerFlows)
            await _makerCheckerHistoryRepository.AddAsync(NewMakerCheckerHistory(item, entity.ReferenceId), cancellationToken);

        return await base.AddAsync(entity, cancellationToken);
    }

    public override TMakerCheckerEntity Update(TMakerCheckerEntity entity)
    {
        var makerCheckerFlows = GetMakerCheckerFlows();
        MakerCheckerRepository<TMakerCheckerEntity>.EntityResetStatus(entity);
        foreach (var item in makerCheckerFlows)
            _makerCheckerHistoryRepository.Add(NewMakerCheckerHistory(item, entity.ReferenceId));

        return base.Update(entity);
    }

    private IQueryable<MakerCheckerFlow> getMakerCheckerFlowQueries()
    {
        return (from definition in _makerCheckerDefinitions
                join flow in _makerCheckerFlows on definition.Id equals flow.MakerCheckerDefinitionId
                where definition.EntityName == _entityName
                  && definition.IsActive && flow.IsActive
                select flow);
    }

    public List<MakerCheckerFlow> GetMakerCheckerFlows() 
    {
        return [.. getMakerCheckerFlowQueries()];
    }

    public Task<List<MakerCheckerFlow>> GetAsyncMakerCheckerFlows(CancellationToken cancellationToken)
    {
         return getMakerCheckerFlowQueries().ToListAsync(cancellationToken);
    }

    private static MakerCheckerHistory NewMakerCheckerHistory(MakerCheckerFlow makerCheckerFlow, Guid referenceId) => new()
    {
        Id = Guid.NewGuid(),
        MakerCheckerFlowId = makerCheckerFlow.Id,
        ReferenceId = referenceId,
        ApproveStatus = ApproveStatus.Pending
    };

    private static void EntityResetStatus(TMakerCheckerEntity entity)
    {
        entity.SetNewReferenceId();
        entity.SetApproveStatus(ApproveStatus.Pending);
    }
}
