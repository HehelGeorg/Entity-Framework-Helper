using EFHelper.Patterns.InfrastructureStrategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFTest.ModuleTest.SpecificationTest;

public class Where_String_Include_Test : IClassFixture<Fixture>
{
    private readonly Fixture _fixture;

    
    public Where_String_Include_Test(Fixture fixture)
    {
        _fixture = fixture;
  
    }
    
    
    [Fact]
    // проверка фильтрации и строкового включения
    public async Task Specification_WhereStringInclude_LoadsRelatedData()
    {
       
        // ARRANGE:
        var alpha = new Company("AlphaCorp");
        var empAlice = new Employee("Alice", "a@a.com", 40) { Company = alpha };

        using (var scopeWrite = _fixture.CreateScope())
        {
            var context = scopeWrite.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Companies.Add(alpha);
            context.Employees.Add(empAlice);
            await context.SaveChangesAsync();
        }

        // ACT / ASSERT:
        using (var scopeRead = _fixture.CreateScope())
        {
            var strategy = scopeRead.ServiceProvider.GetRequiredService<IApplyQueryStrategy<Employee>>();
            var spec = new Where_String_Include_SpecTest();
            var context = scopeRead.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var query = context.Employees.AsQueryable();

            var results = await strategy.Apply(query, spec).ToListAsync();
            var result = Assert.Single(results);

            // ASSERT: Проверка, что Company загружена
            Assert.NotNull(result.Company);
            Assert.Equal("AlphaCorp", result.Company.Name);
        }
    }
}