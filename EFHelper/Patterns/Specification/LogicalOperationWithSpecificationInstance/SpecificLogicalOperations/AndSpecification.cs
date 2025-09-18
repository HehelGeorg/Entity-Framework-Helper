using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFHelper.Patterns;

/// <summary>
/// Использует функционал для
/// Соединения двух деревьев запроса
/// по логическому оператору "И"/"AND"
/// </summary>
public class AndSpecification<TEntity> : Specification<TEntity> where TEntity : class
{
    /// <summary>
    /// Соединяет два дерева запроса по логическому оператору "И"/"AND"
    /// </summary>
    /// <param name="left">Левое дерево</param>
    /// <param name="right">Правое дерево</param>
    public AndSpecification(Specification<TEntity> left, Specification<TEntity> right)
    {
        var logicalOperation = new LogicalOperation<TEntity>(
            left,
            right,
            Expression.AndAlso
        );
    }

    
}

//

