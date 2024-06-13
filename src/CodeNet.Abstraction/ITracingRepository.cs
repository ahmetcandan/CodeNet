using CodeNet.Abstraction.Model;

namespace CodeNet.Abstraction;

public interface ITracingRepository<TTracingEntity> : IBaseRepository<TTracingEntity> where TTracingEntity : class, ITracingEntity
{
}
