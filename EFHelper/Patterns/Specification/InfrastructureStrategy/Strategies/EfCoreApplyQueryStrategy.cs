using EFHelper.Patterns.Builders;
using Microsoft.EntityFrameworkCore;

namespace EFHelper.Patterns.InfrastructureStrategy;


/// <summary>
/// Реализация стратегии интерпретации специфкации в запрос
/// для Entity Framework
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class EfCoreApplyQueryStrategy<TEntity> : IApplyQueryStrategy<TEntity> where TEntity : class
{

  
    
        
    /// <summary>
    /// Применяет поля Specification
    /// Для того, чтобы создать новый объект запроса,
    /// который в последующем будет использоваться
    /// провайдером LINQtE
    /// </summary>
    /// <param name="queryable">
    /// Принимает необязательный параметр queryable,
    /// Если не требуется добавить запрос, генерируемый специфкацией,
    /// К существующему объекту запроса, то лучше позволить Apply()
    /// Самостоятельно сгенерировать объект IQueryable
    /// </param>
    /// <returns>
    /// Возвращает итоговый объект запроса,
    /// Сгенерированный относительно полей Specification
    /// </returns>
    public virtual IQueryable<TEntity> Apply(IQueryable<TEntity> queryable , Specification<TEntity> specification)
    {
        
        
        // Применяем фильтр
        if (specification.FilterQuery is not null)
        {
            queryable = queryable.Where(specification.FilterQuery);
        }

        // Применяем включения связанных данных
        if (specification.IncludeQueries?.Count > 0)
        {
            queryable = specification.IncludeQueries.Aggregate(queryable,
                (current, includeQuery) => current.Include(includeQuery));
        }
        
        // Применяем строковое включение связанных данных
        if (specification.IncludeStrings?.Count > 0)
        {
            queryable = specification.IncludeStrings.Aggregate(queryable,
                (current, includeString) => current.Include(includeString));
        }
        
        
        // Применяем сортировку 
        // Первый элемент в коллекции OrderBy применяется к одноименному методу LINQ
        // Последующие методы в коллекции применяются к методу через ThenBy посредством Aggregate,
        // который позволяет применить данную функцию относительно каждого элемента коллекции OrderBy после первого 
        if (specification.OrderByQueries?.Count > 0)
        {
            var orderedQueryable = queryable.OrderBy(specification.OrderByQueries.First());
            
            orderedQueryable = specification.OrderByQueries.Skip(1)
                .Aggregate(orderedQueryable, (current, orderQuery) => current.ThenBy(orderQuery));
            
            queryable = orderedQueryable;
        }
        
        // Аналогично сортировки с OrderBy, 
        // только применяются Desc методы-копии OrderBy/ThenBy
        // В остальном реализация сортировки, присоединяемой к объекту запроса 
        // не меняется 
        if (specification.OrderByDescendingQueries?.Count > 0)
        {
            var orderedQueryable = queryable.OrderByDescending(specification.OrderByDescendingQueries.First());
            
            orderedQueryable = specification.OrderByDescendingQueries.Skip(1)
                .Aggregate(orderedQueryable, (current, orderQuery) => current.ThenByDescending(orderQuery));
            
            queryable = orderedQueryable;
        }
        
        
        // Применяем пагинацию
        if (specification.Skip is not null) queryable = queryable.Skip(specification.Skip.Value);
        if (specification.Take is not null) queryable = queryable.Take(specification.Take.Value);

        // Возвращаем итоговый объект запроса, который может использоваться LINQtE 
        // Для обращения к БД 
        return queryable;
    }


    
}

