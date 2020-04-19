using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace YASDM.Api
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }
    }

    public static class PagedListExtensions
    {
        public static async Task<PagedList<T>> ToPagedListAsync<T, TKey>(this IQueryable<T> superset, Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize)
        {
            var count = await superset.CountAsync();
            var items = await superset.OrderBy(keySelector)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);

        }
    }
}