using System.Linq.Expressions;

namespace EFHelper.Patterns.Builders;


/// <summary>
/// Инкапсулирует создание Спецификации на уровне public
/// </summary>
/// <remarks>
/// BUILDER ИСКЛЮЧИТЕЛЬНО ДОМЕННОЙ ОБЛАСТИ
/// В НЕМ ОТСУТСТВУЕТ МЕТОД BUILD()
/// ЗА ЭТО ОТВЕТСТВЕННЫ КОНКРЕТНЫЕ РЕАЛИЗАЦИИ,
/// ОТВЕЧАЮЩИЕ ЗА ПЕРЕВОД ПОЛЕЙ В ЗАПРОС К КОНКРЕТНОМУ ПРОВАЙДЕРУ БД И ПРОЧИХ 
/// </remarks>
/// <typeparam name="TEntity"></typeparam>
public class SpecificationBuilder<TEntity> : Specification<TEntity> where TEntity : class
{

    /// <summary>
    /// Публичное добавление фильтрации
    /// </summary>
    /// <remarks>
    /// Запросы фильтрации не суммируются.
    /// Если вызвать WithFilterQuery() несколько раз,
    /// То прокинутые запросы фильтрации перезапишут друг друга 
    /// </remarks>
    /// <param name="query">Объект запроса фильтрации</param>
    public SpecificationBuilder<TEntity> WithFilterQuery(Expression<Func<TEntity, bool>> query)
    {
        AddFilteringQuery(query);
        return this;
    }
    
    /// <summary>
    /// Публичное добавление запроса включения
    /// </summary>
    /// <remarks>
    /// Запросы суммируются с каждым вызовом
    /// </remarks>
    /// <param name="query">Объект запроса включения</param>
    public SpecificationBuilder<TEntity> WithIncludeQuery(Expression<Func<TEntity, object>> query)
    {
        AddIncludeQuery(query);
        return this;
    }
    
    
  
    /// <summary>
    /// Публичное добавление строковых запросов включения 
    /// </summary>
    /// <remarks>
    /// Запросы суммируются с каждым вызовом
    /// </remarks>
    /// <param name="query">Строковые запросы включения</param>
    public SpecificationBuilder<TEntity> WithStringIncludeQuery(List<string> query)
    {
        AddIncludeString(query);
        return this;
    }
    
    /// <summary>
    /// Публичное добавление запросов первичной сортировки
    /// </summary>
    /// <remarks>
    /// Первичные запросы сортировки не суммируются.
    /// Если WithOrderByQuery() было уже вызвано,
    /// То при новом вызове пробрасывается исключение
    /// </remarks>
    /// <exception cref="Exception">При попытке использования WithOrderByQuery() больше чем раз</exception>
    /// <param name="query">Объект запроса сортировки</param>
    public SpecificationBuilder<TEntity> WithOrderByQuery(Expression<Func<TEntity, object>> query)
    {
        if (OrderByQueries?.Count == 0)
        {
            AddOrderByQuery(query);
            return this;
        }
        else
        {
            throw new Exception("Order queries cannot be changed");
        }
    }
    
    
    /// <summary>
    /// Публичное добавление запросов вторичной сортировки
    /// </summary>
    /// <remarks>
    /// С каждым вызовом WithThenByQuery(),
    /// Запросы суммируются
    /// </remarks>
    /// <param name="query">Объект запроса сортировки</param>
    public SpecificationBuilder<TEntity> WithThenByQuery(Expression<Func<TEntity, object>> query)
    {
        if (OrderByQueries?.Count > 0)
        {
            AddOrderByQuery(query);
            return this;
        }
        else
        {
            throw new Exception("Please add Order By query");
        }
        
    }
    
    
    /// <summary>
    /// Публичное добавление запросов первичной обратной сортировки
    /// </summary>
    /// <remarks>
    /// Первичные запросы обратной сортировки не суммируются.
    /// Если WithOrderByDescendingQuery() было уже вызвано,
    /// То при новом вызове пробрасывается исключение
    /// </remarks>
    /// <exception cref="Exception">При попытке использования WithOrderByDescendingQuery() больше чем раз</exception>
    /// <param name="query">Объект запроса обратной сортировки</param>
    public SpecificationBuilder<TEntity> WithOrderByDescendingQuery(Expression<Func<TEntity, object>> query)
    {
        if (OrderByDescendingQueries?.Count == 0)
        {
            AddOrderByDescendingQuery(query);
            return this;
        }
        else
        {
            throw new Exception("Order Desc queries cannot be changed");
        }
    }
    
    
    
    /// <summary>
    /// Публичное добавление запросов вторичной обратной сортировки
    /// </summary>
    /// <remarks>
    /// С каждым вызовом WithThenByDescendingQuery(),
    /// Запросы суммируются
    /// </remarks>
    /// <param name="query">Объект запроса обратной сортировки</param>
    public SpecificationBuilder<TEntity> WithThenByDescendingQuery(Expression<Func<TEntity, object>> query)
    {
        if (OrderByDescendingQueries?.Count > 0)
        {
            AddOrderByDescendingQuery(query);
            return this;
        }
        else
        {
            throw new Exception("Please add Order Desc By query");
        }
    }
    
    /// <summary>
    /// Публичное указание пропускаемых элементов
    /// </summary>
    /// <param name="skipElements">Значение пропускаемых элементов</param>
    public SpecificationBuilder<TEntity> WithSkip(int skipElements)
    {
        AddPaging(skip: skipElements);
        return this;

    }

    /// <summary>
    /// Публичное указание количества элементов, попадаемых в выборку
    /// </summary>
    /// <param name="takeElements">Значение элементов, которые попадают в выборку</param>
    public SpecificationBuilder<TEntity> WithTake(int  takeElements)
    {
        AddPaging(take: takeElements);
        return this;
    }


    
    
}