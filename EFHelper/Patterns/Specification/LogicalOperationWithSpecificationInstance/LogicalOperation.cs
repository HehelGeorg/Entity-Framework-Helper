using System.Linq.Expressions;

namespace EFHelper.Patterns;



/// <summary>
/// Определяет конструктор, который позволяет соединить два дерева запросов(а именно фильтр) относительно логического оператора
/// С двумя слагаемыми 
/// </summary>
public class LogicalOperation<TEntity> : Specification<TEntity> where TEntity : class  
{

    /// <summary>
    /// Вызывает приватный метод RegisterFilteringQuery(), который соединяет два дерева запроса по логическому оператору
    /// Статических методов-логических-операторов Expression, которые возвращают Binary Expression, прокидывая параметры,
    /// Принимает операторы с двумя слагаемыми 
    /// </summary>
    /// <param name="left">Левое дерево</param>
    /// <param name="right">Правое дерево</param>
    /// <param name="logicalOperator">Логический оператор статического метода Expression</param>
    public LogicalOperation(Specification<TEntity> left, Specification<TEntity> right, 
        Func< Expression, Expression,BinaryExpression> logicalOperator)
    {
        RegisterFilteringQuery(left, right, logicalOperator);
    } 

    

    /// <summary>
    /// Соединяет два дерева запросо по логическому оператору, если IsTwoNotNull() дал положительный результат
    /// </summary>
    /// <param name="left">Левое дерево</param>
    /// <param name="right">Правое дерево</param>
    /// <param name="logicalOperator">Логический оператор статического метода Expression, который возвращает BinaryExpression</param>
    private void  RegisterFilteringQuery(Specification<TEntity> left, 
        Specification<TEntity> right, 
        Func< Expression, Expression,BinaryExpression> logicalOperator)
    {
         if (IsTwoNotNull(left, right))
         {
             var leftExpression = left.FilterQuery;
             var rightExpression = right.FilterQuery;

             var replaceVisitor = new ReplaceExpressionVisitor(rightExpression!.Parameters.Single(),
                 leftExpression!.Parameters.Single());
             var replacedBody = replaceVisitor.Visit(rightExpression.Body);

             var xorExpression = logicalOperator(leftExpression.Body, replacedBody);
             var lambda = Expression.Lambda<Func<TEntity, bool>>(xorExpression, leftExpression.Parameters.Single());

             AddFilteringQuery(lambda);
         }
    }

    
    
    /// <summary>
    /// Проверяет два дерева запроса на null
    /// </summary>
    /// <remarks>
    /// Если два дерева null - false
    /// Если одно из деревьев null - false и прибавляет фильтр того дерва, что имеет значения
    /// Если оба не null - true
    /// </remarks>
    /// <param name="left">Левое дерево</param>
    /// <param name="right">Правое дерево</param>
    /// <returns>Возвращает true, если оба имеют значение, в противном случае false</returns>
    private bool IsTwoNotNull(Specification<TEntity> left, Specification<TEntity> right)
    {
        var leftExpression = left.FilterQuery;
        var rightExpression = right.FilterQuery;

        if (leftExpression is null && rightExpression is null)
        {
            
            return false ;
        }
        
        if (leftExpression is not null && rightExpression is null)
        {
            
            AddFilteringQuery(leftExpression);
            
            return false;
        }
        
        if (leftExpression is null && rightExpression is not null)
        {
            AddFilteringQuery(rightExpression);
            return false;
        }
        
        return  true;
        
    }
    
}