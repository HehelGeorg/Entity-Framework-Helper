namespace EFHelper.Patterns.InfrastructureStrategy;

/// <summary>
/// Абстракция паттерна стратегии для интерпретации специфкации
/// В запрос к конкретным поставщикам ORM
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IApplyQueryStrategy<TEntity> where TEntity : class
{
    /// <summary>
    /// Стратегия вызова интерпретации специфкации в запрос для конкретного провайдера ORM
    /// </summary>
    /// <param name="query">Объект запроса</param>
    /// <param name="specification"> Конкретный случай специфкации</param>
    /// <returns>Объект запроса</returns>
    public IQueryable<TEntity> Apply(IQueryable<TEntity> query, Specification<TEntity> specification);
    
    
}