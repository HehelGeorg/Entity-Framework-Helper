namespace EFTest.ModuleTest.LogicalOperationWithSpecificationTest;
using EFTest.ModuleTest.LogicalOperationWithSpecificationTest.SpecCases;

using EFHelper.Patterns;
using EFHelper.Patterns.InfrastructureStrategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class OrOperator_Test : IClassFixture<Fixture>
{
    private readonly Fixture _fixture;

    public OrOperator_Test(Fixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Specification_Or_FiltersToUnion()
    {
        // ARRANGE: Используем те же данные
        

        // 1. Создайте компанию
        var techCompany = new Company("GlobalTech"); 

        var empAlice = new Employee("Alice", "a@a.com", 40){ Company = techCompany }; // TRUE
        var empBob = new Employee("Bob", "b@b.com", 30){ Company = techCompany };    // TRUE
        var empCharlie = new Employee("Charlie", "c@c.com", 50){ Company = techCompany }; // TRUE
        var empDavid = new Employee("David", "d@d.com", 20){ Company = techCompany };  // FALSE

        using (var scopeWrite = _fixture.CreateScope())
        {
            var context = scopeWrite.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Employees.AddRange(empAlice, empBob, empCharlie, empDavid);
            await context.SaveChangesAsync();
        }

        // ACT: IsActive (>= 40) OR IsJunior (== 30)
        using (var scopeRead = _fixture.CreateScope())
        {
            var strategy = scopeRead.ServiceProvider.GetRequiredService<IApplyQueryStrategy<Employee>>();
            var context = scopeRead.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var query = context.Employees.AsQueryable();

            var left = new IsActiveSpec(); // Age >= 40 (Alice, Charlie)
            var right = new IsJuniorSpec(); // Age == 30 (Bob)
            
            // Композиция: Alice, Bob, Charlie (3 записи)
            var combinedSpec = left.Or(right); 
            
            var results = await strategy.Apply(query, combinedSpec).ToListAsync();

            // ASSERT
            Assert.Equal(3, results.Count);
            // Проверяем, что David (20) отсутствует, а остальные есть
            Assert.DoesNotContain(results, e => e.Name == "David");
        }
    }
}