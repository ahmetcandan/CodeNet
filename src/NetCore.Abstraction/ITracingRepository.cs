using NetCore.Abstraction.Model;

namespace NetCore.Abstraction
{
    public interface ITracingRepository<TTracingEntity> : IBaseRepository<TTracingEntity> where TTracingEntity : class, ITracingEntity
    {
    }
}
