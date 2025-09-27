using EFHelper.Patterns.Builders;
using EFHelper.Patterns.InfrastructureStrategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFTest.ModuleTest.SpecificationBuilderTest;

// Тест билдера спецификаций
public class SpecBuilder_Test : IClassFixture<Fixture>
{
    private readonly Fixture _fixture;

    public SpecBuilder_Test(Fixture fixture)
    {
        _fixture = fixture;
    }

   
    // Тест с фильтрацией, сортировкой, включением и пагининацией
    
    
    
    [Fact]
    // Тест фильтрации, включения, сортировки
    public async Task Builder_FilterIncludeOrdering_BuildsAndExecutesCorrectly()
    {
        // ARRANGE

        var companyA = new Company("Alpha");
        var companyB = new Company("Beta");
        
        // Emp 1: Проходит фильтр, сортируется вторым
        var empAlice = new Employee("Alice", "a@a.com", 40) { Company = companyA }; 
        // Emp 2: Не проходит фильтр
        var empBob = new Employee("Bob", "b@b.com", 30) { Company = companyA }; 
        // Emp 3: Проходит фильтр, сортируется первым
        var empCharlie = new Employee("Charlie", "c@c.com", 50) { Company = companyB }; 

        using (var scopeWrite = _fixture.CreateScope())
        {
            var context = scopeWrite.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Companies.AddRange(companyA, companyB);
            context.Employees.AddRange(empAlice, empBob, empCharlie);
            await context.SaveChangesAsync();
        }

        // ACT
        using (var scopeRead = _fixture.CreateScope())
        {
            var strategy = scopeRead.ServiceProvider.GetRequiredService<IApplyQueryStrategy<Employee>>();
            var context = scopeRead.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var query = context.Employees.AsQueryable();

            // 1. Использование Builder для создания спецификации:
            var specBuilder = new SpecificationBuilder<Employee>()
                .WithFilterQuery(e => e.Age > 35) // Фильтр: 2 сотрудника (Alice, Charlie)
                .WithIncludeQuery(e => e.Company)  // Включение Company
                .WithOrderByQuery(e => e.Name);    // Сортировка: по имени (Alice, Charlie)

            var results = await strategy.Apply(query, specBuilder).ToListAsync();

            // ASSERT
            Assert.Equal(2, results.Count);
            // Проверка сортировки: Alice, Charlie
            Assert.Equal("Alice", results[0].Name); 
            Assert.Equal("Charlie", results[1].Name);
            // Проверка включения
            Assert.NotNull(results[0].Company); 
            Assert.Equal("Alpha", results[0].Company.Name);
            Assert.NotNull(results[1].Company);
            Assert.Equal("Beta", results[1].Company.Name);
        }
    }
    
}