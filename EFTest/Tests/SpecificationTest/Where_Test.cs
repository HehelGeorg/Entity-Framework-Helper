using EFHelper.Patterns.InfrastructureStrategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFTest.ModuleTest.SpecificationTest;

public class Where_Test : IClassFixture<Fixture>
{
    private readonly Fixture _fixture;


    public Where_Test(Fixture fixture)
    {
        _fixture = fixture;

    }

    [Fact]
    // Проверка фильтрации 
    public async Task Specification_Where_FiltersCorrectly()
    {
       
        // ARRANGE: Создание данных (пропустим Companies для краткости, но они должны быть созданы)
        var alpha = new Company("AlphaCorp");
        var empAlice = new Employee("Alice", "a@a.com", 40){Company = alpha};
        var empBob = new Employee("Bob", "b@b.com", 30){Company = alpha};
        var empDavid = new Employee("David", "d@d.com", 50){Company = alpha};

        using (var scopeWrite = _fixture.CreateScope())
        {
            var context = scopeWrite.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Employees.AddRange(empAlice, empBob, empDavid);
            await context.SaveChangesAsync();
        }

        // ACT / ASSERT: Применение спецификации
        using (var scopeRead = _fixture.CreateScope())
        {
            var strategy = scopeRead.ServiceProvider.GetRequiredService<IApplyQueryStrategy<Employee>>();
            var spec = new Where_SpecTest();
            var context = scopeRead.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var query = context.Employees.AsQueryable();
            var results = await strategy.Apply(query, spec).ToListAsync();

            // ASSERT
            Assert.Equal(2, results.Count); // Alice (40), David (50)
            Assert.Contains(results, e => e.Name == "Alice");
            Assert.DoesNotContain(results, e => e.Name == "Bob");
        }
    }
}