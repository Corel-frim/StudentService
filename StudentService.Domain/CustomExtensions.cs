using System;
using System.Linq;
using System.Linq.Expressions;

namespace StudentService.Domain
{
    /// <summary>
    /// Общие экстеншны
    /// </summary>
    public static class CustomExtensions
    {
        /// <summary>
        /// Если по данному условию есть значения, то фильтрует список, иначе оставляет предыдущий
        /// </summary>
        /// <param name="query">Список объектов для фильтрации</param>
        /// <param name="expression">Условие фильтрации</param>
        /// <returns></returns>
        public static IQueryable<T> ThenWhere<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression)
        {
            if (!query.Any(expression)) return query;

            return query.Where(expression);
        }
    }
}
