using CodeNet.EntityFramework.Models;

namespace CodeNet.EntityFramework.Repositories;

public interface ITracingRepository<TTracingEntity> : IBaseRepository<TTracingEntity> where TTracingEntity : class, ITracingEntity
{
}
