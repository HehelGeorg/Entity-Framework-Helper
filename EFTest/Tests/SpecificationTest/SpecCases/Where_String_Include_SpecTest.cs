using EFHelper.Patterns;

namespace EFTest.ModuleTest.SpecificationTest;

public class Where_String_Include_SpecTest : Specification<Employee>
{
    public Where_String_Include_SpecTest()
    {
        AddFilteringQuery(a => a.Age > 35);
        AddIncludeString("Company");
    }
}