using EFHelper.Patterns;

namespace EFTest.ModuleTest.SpecificationTest;

public class Where_Include_Ordering_Pagination_SpecTest : Specification<Employee>
{
    public Where_Include_Ordering_Pagination_SpecTest()
    {
        AddFilteringQuery(a => a.Age > 35);
        AddIncludeQuery(a => a.Company );
        AddOrderByQuery(a => a.Name);
        AddPaging(skip: 1, take: 1);
    }
}