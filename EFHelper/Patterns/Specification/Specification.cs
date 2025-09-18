namespace EFHelper.Patterns;
using System.Linq.Expressions;

using System.Linq.Expressions;

public abstract class Specification<TEntity> : ISpecification<TEntity>
    where TEntity : class
{
    private List<Expression<Func<TEntity, object>>>? _includeQueries;
    
    private List<Expression<Func<TEntity, object>>>? _orderByQueries;
    
    private List<Expression<Func<TEntity, object>>>? _orderByDescendingQueries;
    
    private List<string>? _includeStrings;

    private int? _skip;

    private int? _take;
    
    //Imp
    
    /// <summary>
    ///  Реализация запросов фильтра
    /// </summary>
    public Expression<Func<TEntity, bool>>? FilterQuery { get; private set; }
    
    /// <summary>
    ///  Рализация запросов включения связанных данных
    /// </summary>
    public IReadOnlyCollection<Expression<Func<TEntity, object>>>? IncludeQueries => _includeQueries;
    
    /// <summary>
    ///  Реализация запросов строкового включения связанных данных
    /// </summary>
    public IReadOnlyList<string>?  IncludeStrings => _includeStrings;
    
    /// <summary>
    ///  Реализация запросов сортировки
    /// </summary>
    public IReadOnlyCollection<Expression<Func<TEntity, object>>>? OrderByQueries => _orderByQueries;
    
    /// <summary>
    ///  Реализация запросов обратной сортировки
    /// </summary>
    public IReadOnlyCollection<Expression<Func<TEntity, object>>>? OrderByDescendingQueries => _orderByDescendingQueries;
    
    
    /// <summary>
    ///  Реализация Пагинации: сколько должно быть пропущено 
    /// </summary>
    public int? Skip => _skip;
    
    
    /// <summary>
    ///  Реализация Пагинации: сколько должно быть взято 
    /// </summary>
    public int? Take => _take;

    //End Imp
    
    
    /// <summary>
    /// Реализация дополнительных конструкторов в производных типов
    /// </summary>
    protected Specification(){}

    /// <summary>
    /// Присваивает фильтр
    /// </summary>
    /// <param name="query"></param>
    protected Specification(Expression<Func<TEntity, bool>> query)
    {
        FilterQuery = query;
    }

    /// <summary>
    /// Копирует объект пагинации
    /// </summary>
    /// <param name="specification"></param>
    protected Specification(ISpecification<TEntity> specification)
    {
        //filtering
        FilterQuery = specification.FilterQuery;
        //include
        _includeQueries = specification.IncludeQueries?.ToList();
        _includeStrings = specification.IncludeStrings?.ToList();
        //ordering
        _orderByQueries = specification.OrderByQueries?.ToList();
        _orderByDescendingQueries = specification.OrderByDescendingQueries?.ToList();
        //pagination
        _skip = specification.Skip;
        _take = specification.Take;
    }

    
    /// <summary>
    /// Добавляет фильтрацию
    /// </summary>
    /// <param name="query">объект запроса фильтрации</param>
    protected void AddFilteringQuery(Expression<Func<TEntity, bool>> query)
    {
        FilterQuery = query;
    }

    
    /// <summary>
    /// Добавляет включение связанных данных
    /// </summary>
    /// <param name="query">объект запроса  включения</param>
    protected void AddIncludeQuery(Expression<Func<TEntity, object>> query)
    {
        _includeQueries ??= new();
        _includeQueries.Add(query);
    }

    /// <summary>
    /// Добавляет строковые включения
    /// </summary>
    /// <param name="query">список строковых запросов включений</param>
    protected void AddIncludeString(List<string> query)
    {
        _includeStrings ??= new();
        _includeStrings.AddRange(query);
    }
    
    /// <summary>
    /// Добавляет  сортировку
    /// </summary>
    /// <param name="query">объект запроса  сортировки</param>
    protected void AddOrderByQuery(Expression<Func<TEntity, object>> query)
    {
        _orderByQueries ??= new();
        _orderByQueries.Add(query);
    }

    /// <summary>
    /// Добавляет обратную сортировку
    /// </summary>
    /// <param name="query">объект запроса  обратной сортировки</param>
    protected void AddOrderByDescendingQuery(Expression<Func<TEntity, object>> query)
    {
        _orderByDescendingQueries ??= new();
        _orderByDescendingQueries.Add(query);
    }

    /// <summary>
    /// Добавляет пагинацию
    /// </summary>
    /// <param name="skip">Сколько пропустить</param>
    /// <param name="take">Сколько вставить</param>
    public void AddPaging(int skip, int take)
    {
        _skip = skip;
        _take = take;
    }
    
    
}
