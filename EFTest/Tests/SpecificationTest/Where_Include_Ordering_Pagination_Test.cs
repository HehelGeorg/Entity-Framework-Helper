using EFHelper.Patterns.InfrastructureStrategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFTest.ModuleTest.SpecificationTest;

public class Where_Include_Ordering_Pagination_Test : IClassFixture<Fixture>
{
    private readonly Fixture _fixture;

    
    public Where_Include_Ordering_Pagination_Test(Fixture fixture)
    {
        _fixture = fixture;
  
    }
    
    [Fact]
    // проверка фильтрации, включения, сортировки и пагинации
    public async Task Specification_Full_FiltersSortsAndPaginatesCorrectly()
    {
        // ARRANGE
        var company = new Company("TechCorp");
        var empAlice = new Employee("Alice", "a@a.com", 40) { Company = company };
        var empBob = new Employee("Bob", "b@b.com", 30) { Company = company };
        var empCharlie = new Employee("Charlie", "c@c.com", 45) { Company = company };
        var empDavid = new Employee("David", "d@d.com", 50) { Company = company };

        using (var scopeWrite = _fixture.CreateScope())
        {
            var context = scopeWrite.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Companies.Add(company);
            context.Employees.AddRange(empAlice, empBob, empCharlie, empDavid);
            await context.SaveChangesAsync();
        }

        // ACT
        using (var scopeRead = _fixture.CreateScope())
        {
           
            var strategy = scopeRead.ServiceProvider.GetRequiredService<IApplyQueryStrategy<Employee>>();
            // Отфильтрованные и отсортированные: Alice (40), Charlie (45), David (50)
            // Пропускаем 1, берём 1 -> получаем Charlie
            var spec = new Where_Include_Ordering_Pagination_SpecTest(); 
            var context = scopeRead.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var query = context.Employees.AsQueryable();

            var results = await strategy.Apply(query, spec).ToListAsync();

            // ASSERT
            Assert.Single(results);
            Assert.Equal("Charlie", results[0].Name);
            Assert.NotNull(results[0].Company);
        }
    }
}