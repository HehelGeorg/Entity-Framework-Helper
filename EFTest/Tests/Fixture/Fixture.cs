using EFHelper.Patterns.InfrastructureStrategy;
using EFTest.ModuleTest;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


// Фикстура, управляющая DI-контейнером и подключением к БД
public class Fixture : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly IServiceProvider _serviceProvider;

    public Fixture()
    {
        // 1. Настройка подключения (остается открытым)
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

;
 

    // 2. Настройка   bContextOptions
    var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseSqlite(_connection)
        .LogTo(Console.WriteLine, LogLevel.Debug)
        .EnableSensitiveDataLogging()
        .Options;

        // 3. Создание ServiceCollection и регистрация зависимостей
        var services = new ServiceCollection();

        // Если вам нужен scoped
        // Возьмите ApplicationDbContext c dbContextOptions
        services.AddScoped(_ => new ApplicationDbContext(dbContextOptions));
        // Если вам нужна Стратегия
        // Возьмите efcore-стратегию
        services.AddScoped(typeof(IApplyQueryStrategy<>), typeof(EfCoreApplyQueryStrategy<>)); 
        
        // 4. Построение ServiceProvider
        _serviceProvider = services.BuildServiceProvider();
        
        // 5. Обеспечение создания схемы (выполняется один раз)
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        }
    }

    // Метод, который тесты будут использовать для получения изолированного Scope
    public IServiceScope CreateScope()
    {
        return _serviceProvider.CreateScope();
    }
    

 

    // Очистка
    public void Dispose()
    {
        _connection.Dispose();
    }
}