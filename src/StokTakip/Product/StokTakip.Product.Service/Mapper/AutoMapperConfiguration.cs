using AutoMapper;
using System.Reflection;

namespace StokTakip.Product.Service.Mapper;

public class AutoMapperConfiguration : IAutoMapperConfiguration
{
    private readonly AutoMapper.Mapper _mapper;
    private readonly MapperConfiguration _mapperConfiguration;

    public AutoMapperConfiguration()
    {
        _mapperConfiguration = new MapperConfiguration(c =>
        {
            c.AddMaps(Assembly.GetExecutingAssembly());
            c.AllowNullCollections = true;
            c.AllowNullDestinationValues = true;
        });

        _mapper = new AutoMapper.Mapper(_mapperConfiguration);
    }

    public IEnumerable<TReturn> MapCollection<TMap, TReturn>(IEnumerable<TMap> expression)
        where TMap : class
        where TReturn : class => _mapper.Map<IEnumerable<TMap>, IEnumerable<TReturn>>(expression);

    public TReturn MapObject<TMap, TReturn>(TMap obj)
        where TMap : class
        where TReturn : class => _mapper.Map<TMap, TReturn>(obj);
}
