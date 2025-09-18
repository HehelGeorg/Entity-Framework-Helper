using System.Linq.Expressions;

namespace EFHelper.Patterns;

/// <summary>
/// Использует функционал для
/// Соединения двух деревьев запроса
/// по логическому оператору "ИСКЛЮЧАЮЩИЕ ИЛИ"/"EXCLUSIVE OR"/"XOR"
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class XorSpecification<TEntity> : Specification<TEntity> where TEntity : class
{

    /// <summary>
    /// Соединяет два дерева запроса по логическому оператору "ИСКЛЮЧАЮЩИЕ ИЛИ"/"EXCLUSIVE OR"/"XOR"
    /// </summary>
    /// <param name="left">Левое дерево</param>
    /// <param name="right">Правое дерево</param>
    public XorSpecification(Specification<TEntity> left, Specification<TEntity> right)
    {
        var logicalOperation = new LogicalOperation<TEntity>(
            left,
            right,
            Expression.ExclusiveOr
        );
    }
 
}

// 


