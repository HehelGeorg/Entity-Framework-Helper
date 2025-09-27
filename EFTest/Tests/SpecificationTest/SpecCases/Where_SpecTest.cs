using EFHelper.Patterns;

namespace EFTest.ModuleTest.SpecificationTest;

public class Where_SpecTest : Specification<Employee>
{
    public Where_SpecTest()
    {
        AddFilteringQuery(a => a.Age > 35);
    }
}