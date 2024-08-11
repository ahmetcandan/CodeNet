namespace CodeNetUI_Example.Models;

public class PagingResponse<TEntity>
{
    public List<TEntity> List { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageCount { get; set; }
}
