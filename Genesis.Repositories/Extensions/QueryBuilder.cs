using Genesis.Models.DTO;
using Genesis.Repositories.Expressions;
using System.Linq.Expressions;

namespace Genesis.Repositories.Extensions
{
    public static class QueryBuilder
    {
        public static IQueryable<T> AddSearchParams<T>(this IQueryable<T> query, SearchParams searchParams)
        {
            Expression<Func<T, bool>> filterExpression = FilterBuilder.BuildExpression<T>(searchParams.Filters);

            query = query.Where(filterExpression);

            if (!string.IsNullOrEmpty(searchParams.OrderBy))
            {
                Expression<Func<T, object>> orderByExpression = OrderByBuilder.BuildExpression<T>(searchParams.OrderBy);
                query = searchParams.IsDescending ? query.OrderByDescending(orderByExpression) : query.OrderBy(orderByExpression);
            }

            if (searchParams.Pagination != null)
            {
                var pagination = searchParams.Pagination;
                query = query.Skip((pagination.CurrentPage ?? 1 - 1) * pagination.RecordsPerPage)
                             .Take(pagination.RecordsPerPage);
            }

            return query;
        }
    }
}
