using System.Linq.Expressions;

namespace EFHelper.Patterns;

/// <summary>
/// Статический класс для методов расширения к базовму классу Specification
/// По логическим операциям с текущим и правым деревьем(то, что прокидывается через параметр
/// </summary>
public static class SpecificationExtensions
{
    
    /// <summary>
    /// Соединяет дерево к текущему 
    /// по логическому оператору "И"/"AND"
    /// </summary>
    /// <param name="left">Текущее дерево</param>
    /// <param name="right">Соединяемое дерево по логическому оператору</param>
    /// <returns></returns>
    public static Specification<TEntity> And<TEntity>(this Specification<TEntity> left, Specification<TEntity> right)
        where TEntity : class
    {
        return new LogicalOperation<TEntity>(left, right, Expression.And); 
    }
    
    /// <summary>
    /// Соединяет дерево к текущему 
    /// по логическому оператору "ИЛИ"/"OR"
    /// </summary>
    /// <param name="left">Текущее дерево</param>
    /// <param name="right">Соединяемое дерево по логическому оператору</param>
    /// <returns></returns>
    public static Specification<TEntity> Or<TEntity>(this Specification<TEntity> left, Specification<TEntity> right)
        where TEntity : class
    { 
        return new LogicalOperation<TEntity>(left, right, Expression.OrElse); 
    }
    

    
}