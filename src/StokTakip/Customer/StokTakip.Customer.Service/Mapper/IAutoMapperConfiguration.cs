namespace StokTakip.Customer.Service.Mapper;

public interface IAutoMapperConfiguration
{
    TReturn MapObject<TMap, TReturn>(TMap obj) where TMap : class where TReturn : class;
    IEnumerable<TReturn> MapCollection<TMap, TReturn>(IEnumerable<TMap> expression) where TMap : class where TReturn : class;
}