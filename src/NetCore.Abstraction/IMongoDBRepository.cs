﻿using System.Linq.Expressions;

namespace NetCore.Abstraction;

public interface IMongoDBRepository<TModel> where TModel : class, new()
{
    public List<TModel> GetList(Expression<Func<TModel, bool>> filter);

    public TModel GetById(Expression<Func<TModel, bool>> filter);

    public TModel Create(TModel model);

    public void Update(Expression<Func<TModel, bool>> filter, TModel model);

    public void Delete(Expression<Func<TModel, bool>> filter);

    public bool Exists(Expression<Func<TModel, bool>> filter);

    public long Count(Expression<Func<TModel, bool>> filter);
}
