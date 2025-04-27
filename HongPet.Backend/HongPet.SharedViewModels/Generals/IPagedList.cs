using System.Collections.Generic;

namespace HongPet.SharedViewModels.Generals
{
    public interface IPagedList<T>
    {
        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public bool HasPrevious { get; }
        public bool HasNext { get; }
        public List<T> Items { get; }
    }
}
