using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCore.Abstraction
{
    public interface INoSqlAsyncRepository<TModel> //where TModel : new()
    {
        public Task<List<TModel>> GetListAsync();

        public Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> filter);

        public Task<TModel> GetByIdAsync(string id);

        public Task<TModel> CreateAsync(TModel model);

        public Task UpdateAsync(string id, TModel model);

        public Task DeleteAsync(TModel model);

        public Task DeleteAsync(string id);

        public Task<bool> ContainsIdAsync(string id);
    }
}
