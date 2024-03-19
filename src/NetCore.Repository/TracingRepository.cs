using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Repository;

public abstract class TracingRepository<TTracingEntity>(DbContext dbContext, IIdentityContext identityContext) : BaseRepository<TTracingEntity>(dbContext), ITracingRepository<TTracingEntity> where TTracingEntity : class, ITracingEntity
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
        tEntity.CreatedUser = identityContext?.UserName;
    }

    protected void SetModifytorInfo(TTracingEntity tEntity)
    {
        tEntity.ModifiedDate = DateTime.Now;
        tEntity.ModifiedUser = identityContext?.UserName;
    }
}
