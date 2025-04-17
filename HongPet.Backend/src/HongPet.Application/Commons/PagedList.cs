using Domain.Entities.Commons;

namespace HongPet.Application.Commons;
public class PagedList<TEntity> : IPagedList<TEntity>
{
    public int CurrentPage { get; private set; } = 1;

    public int TotalPages { get; private set; } = 1;

    public int PageSize { get; private set; } = 1;

    public int TotalCount { get; private set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;

    public List<TEntity> Items { get; private set; } = default!;

    public PagedList() { } // for unit test
    public PagedList(IEnumerable<TEntity> items, 
        int count, int pageIndex, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = items.ToList();
    }
}
