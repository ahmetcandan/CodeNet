namespace CodeNet.EntityFramework.Models;

public class PagingList<TEntity>
{
    public List<TEntity> List { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageCount { get; set; }
}
