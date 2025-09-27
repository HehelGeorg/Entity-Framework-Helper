
namespace EFTest.ModuleTest.LogicalOperationWithSpecificationTest;
using EFTest.ModuleTest.LogicalOperationWithSpecificationTest.SpecCases;

using EFHelper.Patterns;
using EFHelper.Patterns.InfrastructureStrategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;


public class AndOperator_Test : IClassFixture<Fixture>
{
    private readonly Fixture _fixture;

    public AndOperator_Test(Fixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Specification_And_FiltersToIntersection()
    {
        // ARRANGE
        var techCompany = new Company("GlobalTech"); 

        var empAlice = new Employee("Alice", "a@a.com", 40){ Company = techCompany }; // 40 >= 40 AND 40 <= 45 -> TRUE
        var empBob = new Employee("Bob", "b@b.com", 30){ Company = techCompany };    // FALSE
        var empCharlie = new Employee("Charlie", "c@c.com", 50){ Company = techCompany }; // FALSE
        var empDavid = new Employee("David", "d@d.com", 20){ Company = techCompany };  // FALSE

        using (var scopeWrite = _fixture.CreateScope())
        {
            var context = scopeWrite.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Employees.AddRange(empAlice, empBob, empCharlie, empDavid);
            await context.SaveChangesAsync();
        }

        // ACT: IsActive (>= 40) AND IsManager (<= 45)
        using (var scopeRead = _fixture.CreateScope())
        {
            var strategy = scopeRead.ServiceProvider.GetRequiredService<IApplyQueryStrategy<Employee>>();
            var context = scopeRead.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var query = context.Employees.AsQueryable();

            var left = new IsActiveSpec(); // Age >= 40 (Alice, Charlie)
            var right = new IsManagerSpec(); // Age <= 45 (Alice, Bob, David)
            
            // Композиция: Только Alice (40)
            var combinedSpec = left.And(right); 
            
            var results = await strategy.Apply(query, (Specification<Employee>)combinedSpec).ToListAsync();

            // ASSERT
            Assert.Single(results);
            Assert.Equal("Alice", results[0].Name);
        }
    }
}