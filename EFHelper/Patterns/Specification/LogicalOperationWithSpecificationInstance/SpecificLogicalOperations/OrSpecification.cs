using System.Linq.Expressions;

namespace EFHelper.Patterns;

/// <summary>
/// Использует функционал для
/// Соединения двух деревьев запроса
/// по логическому оператору "ИЛИ"/"OR"
/// </summary>
public class OrSpecification<TEntity> : Specification<TEntity> where TEntity : class
{
    /// <summary>
    /// Соединяет два дерева запроса по логическому оператору "ИЛИ"/"OR"
    /// </summary>
    /// <param name="left">Левое дерево</param>
    /// <param name="right">Правое дерево</param>
    public OrSpecification(Specification<TEntity> left, Specification<TEntity> right)
    {
        var logicalOperation = new LogicalOperation<TEntity>(
            left,
            right,
            Expression.Or
        );
    }

}

//

