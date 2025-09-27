using EFHelper.Patterns.InfrastructureStrategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFTest.ModuleTest.SpecificationTest;

public class Where_Include_Ordering_Test : IClassFixture<Fixture>
{
    private readonly Fixture _fixture;

    
    public Where_Include_Ordering_Test(Fixture fixture)
    {
        _fixture = fixture;
  
    }
    
    [Fact]
    //проверка фильтрации, включения и  сортировки
    public async Task Specification_WhereIncludeOrderingAsc_FiltersAndSortsCorrectly()
    {
       
        // ARRANGE
        var company = new Company("TechCorp");
        var empAlice = new Employee("Alice", "a@a.com", 40) { Company = company };
        var empBob = new Employee("Bob", "b@b.com", 30) { Company = company }; // Не пройдёт фильтр
        var empDavid = new Employee("David", "d@d.com", 50) { Company = company };

        using (var scopeWrite = _fixture.CreateScope())
        {
            var context = scopeWrite.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Companies.Add(company);
            context.Employees.AddRange(empAlice, empBob, empDavid);
            await context.SaveChangesAsync();
        }

        // ACT
        using (var scopeRead = _fixture.CreateScope())
        {
            var strategy = scopeRead.ServiceProvider.GetRequiredService<IApplyQueryStrategy<Employee>>();
            var spec = new Where_Include_Ordering_SpectTest();
            var context = scopeRead.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var query = context.Employees.AsQueryable();

            var results = await strategy.Apply(query, spec).ToListAsync();

            // ASSERT
            Assert.Equal(2, results.Count);
            // Ожидаемый порядок: Alice, David (по имени)
            Assert.Equal("Alice", results[0].Name);
            Assert.Equal("David", results[1].Name);
            // Проверяем, что связанные данные загружены
            Assert.NotNull(results[0].Company);
            Assert.NotNull(results[1].Company);
        }
    }
}