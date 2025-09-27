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
        AddIncludeString(query.ToArray());
        return this;
    }
    
    /// <summary>
    /// Публичное добавление запросов сортировки
    /// </summary>
    ///  <remarks>
    /// К первому добавленному запросу будет применена OrderBY
    /// К последующим ThenBy
    /// </remarks>
    /// <param name="query">Запрос сортировки</param>
    /// <returns>Текущий объект спецификации с добавленной сортировкой</returns>
    public SpecificationBuilder<TEntity> WithOrderByQuery(Expression<Func<TEntity, object>> query)
    {
    
            AddOrderByQuery(query);
            return this;
  
    }
    
    
  
    
    /// <summary>
    /// Публичное добавление запросов обратной сортировки
    /// </summary>
    /// <remarks>
    /// К первому запросу будет применена OrderDescBY
    /// К последующим ThenDescBy
    /// </remarks>
    /// <param name="query">Запрос обратной сортировки</param>
    /// <returns>Текущий объект спецификации с добавленной обратной сортировкой</returns>
    public SpecificationBuilder<TEntity> WithOrderByDescendingQuery(Expression<Func<TEntity, object>> query)
    {
            AddOrderByDescendingQuery(query);
            return this;
    
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