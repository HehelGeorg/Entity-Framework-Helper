namespace EFHelper.Patterns;


/// <summary>
/// Интерфейс репозитория, описывающий стандартные методы CRUD
/// На основе паттерна Specification
/// </summary>
/// <example>
/// Ввиду использования экземпляров спецификации, описывающих конкретный запрос,
/// на репозиторий остаётся ответственность всецело быть мостом между доменом и инфрастркутурой
/// Там, где он очернял домен/слой приложения определенной путаницей между слоями, теперь ответственен
/// Specification, он берет ответственность за конкретный вид запросов, сохраняя гибкость и изящество выбора конкретной инфраструктуры,
/// Для более подробного пояснения и объяснений преимуществ такого подхода обращайтесь на страницу репозитория: https://github.com/HehelGeorg/Entity-Framework-Helper 
/// </example>
/// <typeparam name="TEntity"></typeparam>
public interface IRepositoryBasedSpecification<TEntity> where TEntity : class
{
    /// <summary>
    /// Добавление объекта в БД
    /// </summary>
    /// <param name="entity">объект</param>
    /// <returns></returns>
    public IResult Add(TEntity entity);

    
    /// <summary>
    /// Получение выборки на основе спецификации 
    /// </summary>
    /// <param name="specification">Экземпляр специфкации, описывающий запрос получения данных, для дальнейшего предоставления клиентскому коду</param>
    /// <returns></returns>
    public IResult Get(Specification<TEntity> specification);
    
    /// <summary>
    /// Удаление выборки на основе спецификации 
    /// </summary>
    /// <param name="specification">Экземпляр специфкации, описывающий запрос получения данных, для дальнейшего удаления</param>
    /// <returns></returns>
    public IResult Delete(Specification<TEntity> specification);
    
    /// <summary>
    /// Изменение выборки на основе спецификации 
    /// </summary>
    /// <param name="specification">Экземпляр специфкации, описывающий запрос получения данных, для дальнейшего изменения</param>
    /// <param name="toUpdate">Объект описывающий изменения полученных данных</param>
    /// <returns></returns>
    public IResult Update(Specification<TEntity> specification, TEntity toUpdate);
    
}