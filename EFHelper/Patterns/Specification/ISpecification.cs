namespace EFHelper.Patterns;
using System.Linq.Expressions;





///<summary>
/// Интерфейс, определяющий поля спецификации,
/// Для реализации полей различных типов запросов: фильтрация, включение связанных данных, сортировки, пагинации.
/// для дальнейшего обращения суммарного запроса к провайдеру БД(ORM) 
///</summary>
public interface ISpecification<TEntity>{



    /// <summary>
    /// Объект запроса фильтрации
    /// </summary>
    Expression<Func<TEntity, bool>>
        FilterQuery { get; }



    /// <summary>
    ///  Объект запросов включений
    /// </summary>
    IReadOnlyCollection<Expression<Func<TEntity, object>>>?
        IncludeQueries { get; }



    /// <summary>
    ///  Объект запросов строковых включений
    /// </summary>
    IReadOnlyList<string>?
        IncludeStrings { get; }



    /// <summary>
    /// Объект запросов сортировок
    /// </summary>
    IReadOnlyCollection<Expression<Func<TEntity, object>>>? OrderByQueries { get; }
    
    /// <summary>
    /// Объект запросов обратной сортировки
    /// </summary>
    IReadOnlyCollection<Expression<Func<TEntity, object>>>? OrderByDescendingQueries { get; }


    /// <summary>
    /// Пагинация: сколько пропустить
    /// </summary>
    int? Skip { get; }

    /// <summary>
    /// Пагинация: сколько взять
    /// </summary>
    int? Take { get; }



}


