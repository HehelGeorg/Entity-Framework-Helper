using EFHelper.Patterns;

namespace EFTest.ModuleTest.SpecificationTest;

public class Where_Include_Ordering_SpectTest : Specification<Employee>
{
    public Where_Include_Ordering_SpectTest()
    {
        AddFilteringQuery(a => a.Age > 35);
        AddIncludeQuery(a => a.Company );
        AddOrderByQuery(a => a.Name);
    }  
}

public class Where_Include_OrderingDesc_SpectTest : Specification<Employee>
{
    public Where_Include_OrderingDesc_SpectTest()
    {
        AddFilteringQuery(a => a.Age > 35);
        AddIncludeQuery(a => a.Company );
        AddOrderByDescendingQuery(a => a.Name);
    }
}