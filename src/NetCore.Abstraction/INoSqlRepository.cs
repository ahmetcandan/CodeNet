using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetCore.Abstraction
{
    public interface INoSqlRepository<TModel> //where TModel : new()
    {
        public List<TModel> GetList();

        public List<TModel> GetList(Expression<Func<TModel, bool>> filter);

        public TModel GetById(string id);

        public TModel Create(TModel model);

        public void Update(string id, TModel model);

        public void Delete(TModel model);

        public void Delete(string id);

        public bool ContainsId(string id);
    }
}
