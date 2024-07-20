using CodeNet.Core;
using CodeNet.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.EntityFramework.Repositories;

public abstract class TracingRepository<TTracingEntity>(DbContext dbContext, ICodeNetContext codeNetContext) : BaseRepository<TTracingEntity>(dbContext), ITracingRepository<TTracingEntity>
    where TTracingEntity : class, ITracingEntity
{
    public override TTracingEntity Add(TTracingEntity entity)
    {
        SetCreatorInfo(entity);
        return base.Add(entity);
    }

    public override Task<TTracingEntity> AddAsync(TTracingEntity entity)
    {
        return AddAsync(entity, CancellationToken.None);
    }

    public override Task<TTracingEntity> AddAsync(TTracingEntity entity, CancellationToken cancellationToken)
    {
        SetCreatorInfo(entity);
        return base.AddAsync(entity, cancellationToken);
    }

    public override IEnumerable<TTracingEntity> AddRange(IEnumerable<TTracingEntity> entities)
    {
        foreach (var entity in entities)
            SetCreatorInfo(entity);

        return base.AddRange(entities);
    }

    public override Task<IEnumerable<TTracingEntity>> AddRangeAsync(IEnumerable<TTracingEntity> entities)
    {
        return AddRangeAsync(entities, CancellationToken.None);
    }

    public override Task<IEnumerable<TTracingEntity>> AddRangeAsync(IEnumerable<TTracingEntity> entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
            SetCreatorInfo(entity);

        return base.AddRangeAsync(entities, cancellationToken);
    }

    public override TTracingEntity Update(TTracingEntity entity)
    {
        SetModifytorInfo(entity);
        return base.Update(entity);
    }

    public override IEnumerable<TTracingEntity> UpdateRange(IEnumerable<TTracingEntity> entities)
    {
        foreach (var entity in entities)
            SetModifytorInfo(entity);

        return base.UpdateRange(entities);
    }

    protected void SetCreatorInfo(TTracingEntity tEntity)
    {
        tEntity.CreatedDate = DateTime.Now;
        tEntity.CreatedUser = codeNetContext?.UserName ?? string.Empty;
    }

    protected void SetModifytorInfo(TTracingEntity tEntity)
    {
        tEntity.ModifiedDate = DateTime.Now;
        tEntity.ModifiedUser = codeNetContext?.UserName;
    }
}
