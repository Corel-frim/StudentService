using StudentService.Data.Models;
using StudentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StudentService.Data
{
    /// <summary>
    /// Экстеншны, затрагивающие слой данных
    /// </summary>
    public static class DataLayerExtensions
    {
        /// <summary>
        /// Получение списка ExpressionTree, которые можно затем кидать в LINQ, на основе фильтров
        /// </summary>
        /// <param name="filter">Фильтры</param>
        /// <returns>Список ExpressionTree</returns>
        public static IEnumerable<Expression<Func<Student, bool>>> GetConditionExpressions(this StudentListFilter filter)
        {
            var allConditions = new List<(bool Condition, Expression<Func<Student, bool>> Expression)>
            {
                (!string.IsNullOrWhiteSpace(filter.FirstName), s => s.FirstName.Contains(filter.FirstName)),
                (!string.IsNullOrWhiteSpace(filter.LastName), s => s.LastName.Contains(filter.LastName)),
                (!string.IsNullOrWhiteSpace(filter.MiddleName), s => s.MiddleName.Contains(filter.MiddleName)),
                (!string.IsNullOrWhiteSpace(filter.Nickname), s => s.Nickname.Contains(filter.Nickname)),
                (filter.Sex != null, s => s.Sex == filter.Sex),
                (!string.IsNullOrWhiteSpace(filter.GroupName), s => s.Groups.Any(g => g.Group.Name.Contains(filter.GroupName))),
            };

            foreach (var condition in allConditions.Where(c => c.Condition)) yield return condition.Expression;
        }

        /// <summary>
        /// Добавление пагинации к запросу
        /// </summary>
        /// <param name="query">Текущий запрос</param>
        /// <param name="take">Кол-во запрашиваемых сущностей</param>
        /// <param name="skip">Кол-во пропускаемых сущностей</param>
        /// <returns>Запрос с пагинацией</returns>
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, int take, int skip = 0)
        {
            if (skip != 0) query = query.Skip(skip);
            if (take != 0) query = query.Take(take);

            return query;
        }
    }
}
