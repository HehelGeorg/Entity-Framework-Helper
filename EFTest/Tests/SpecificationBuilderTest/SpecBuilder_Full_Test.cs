using EFHelper.Patterns.Builders;
using EFHelper.Patterns.InfrastructureStrategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFTest.ModuleTest.SpecificationBuilderTest;

public class SpecBuilder_Full_Test : IClassFixture<Fixture>
{
    private readonly Fixture _fixture;

    public SpecBuilder_Full_Test(Fixture fixture)
    {
        _fixture = fixture;
    }
    [Fact]
    public async Task Builder_FullQuery_FiltersSortsAndPaginatesCorrectly()
    {
        // ARRANGE

        var company = new Company("TechCorp");
        var empAlice = new Employee("Alice", "a@a.com", 40) { Company = company };
        var empBob = new Employee("Bob", "b@b.com", 30) { Company = company }; // Не пройдёт
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
            var context = scopeRead.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var query = context.Employees.AsQueryable();

            // Отфильтрованные и отсортированные (по имени ASC): Alice, Charlie, David
            // Мы пропускаем 1, берем 1. Ожидаемый результат: Charlie
            var specBuilder = new SpecificationBuilder<Employee>()
                .WithFilterQuery(e => e.Age > 35)
                .WithIncludeQuery(e => e.Company)
                .WithOrderByQuery(e => e.Name)
                .WithSkip(1)
                .WithTake(1);

            var results = await strategy.Apply(query, specBuilder).ToListAsync();

            // ASSERT
            Assert.Single(results);
            Assert.Equal("Charlie", results[0].Name);
            Assert.NotNull(results[0].Company);
        }
    }

}