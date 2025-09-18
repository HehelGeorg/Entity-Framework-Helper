using Microsoft.EntityFrameworkCore;

namespace EFHelper.Patterns;


/// <summary>
///  Конкретная реализация Specification,
///  Определяющий метод apply() для создания объекта запроса
///  К провайдеру LINQtE 
/// </summary>
/// <example>
/// В случае, если потребуется
/// исползования продвинутых способов фильтрации, пагинации и т.д.
/// скажем, курсовой пагинации, то следует описать отдельный тип,
/// наследуемый от EfCoreSpecification.
/// К примеру, CursorPaginationSpecification
/// </example>
/// <typeparam name="TEntity">
/// тип, относительно которого и применяются методы-поля спецификации
/// </typeparam>
public class EfCoreSpecification<TEntity> : Specification<TEntity> 
    where  TEntity : class
{
    
    
    /// <summary>
    /// Копирует объект спецификации
    /// </summary>
    /// <param name="specification"></param>
    public EfCoreSpecification(Specification<TEntity> specification) : base(specification){}
    
    
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
    /// Возвращает итоговый объект запроса запроса,
    /// Сгенерированный относительно полей Specification
    /// </returns>
    //
    //
    //
    //
    public virtual IQueryable<TEntity> Apply(IQueryable<TEntity> queryable )
    {
        
        
        // Применяем фильтр
        if (FilterQuery is not null)
        {
            queryable = queryable.Where(FilterQuery);
        }

        // Применяем включения связанных данных
        if (IncludeQueries?.Count > 0)
        {
            queryable = IncludeQueries.Aggregate(queryable,
                (current, includeQuery) => current.Include(includeQuery));
        }
        
        // Применяем строковое включение связанных данных
        if (IncludeStrings?.Count > 0)
        {
            queryable = IncludeStrings.Aggregate(queryable,
                (current, includeString) => current.Include(includeString));
        }
        
        
        // Применяем сортировку 
        // Первый элемент в коллекции OrderBy применяется к одноименному методу LINQ
        // Последующие методы в коллекции применяются к методу через ThenBy посредством Aggregate,
        // который позволяет применить данную функцию относительно каждого элемента коллекции OrderBy после первого 
        if (OrderByQueries?.Count > 0)
        {
            var orderedQueryable = queryable.OrderBy(OrderByQueries.First());
            
            orderedQueryable = OrderByQueries.Skip(1)
                .Aggregate(orderedQueryable, (current, orderQuery) => current.ThenBy(orderQuery));
            
            queryable = orderedQueryable;
        }
        
        // Аналогично сортировки с OrderBy, 
        // только применяются Desc методы-копии OrderBy/ThenBy
        // В остальном реализация сортировки, присоединяемой к объекту запроса 
        // не меняется 
        if (OrderByDescendingQueries?.Count > 0)
        {
            var orderedQueryable = queryable.OrderByDescending(OrderByDescendingQueries.First());
            
            orderedQueryable = OrderByDescendingQueries.Skip(1)
                .Aggregate(orderedQueryable, (current, orderQuery) => current.ThenByDescending(orderQuery));
            
            queryable = orderedQueryable;
        }
        
        
        // Применяем пагинацию
        if (Skip is not null) queryable = queryable.Skip(Skip.Value);
        if (Take is not null) queryable = queryable.Take(Take.Value);

        // Возвращаем итоговый объект запроса, который может использоваться LINQtE 
        // Для обращения к БД 
        return queryable;
    }
    
}