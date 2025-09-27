using EFHelper.Patterns;

namespace EFTest.ModuleTest.SpecificationTest;

public class Where_Include_SpecTest : Specification<Employee>
{
    public Where_Include_SpecTest()
    {
        AddFilteringQuery(a => a.Age > 35);
        AddIncludeQuery(a => a.Company );
    }
}