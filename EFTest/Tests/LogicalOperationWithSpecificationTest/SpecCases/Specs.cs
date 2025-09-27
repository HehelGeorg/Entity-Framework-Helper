using EFHelper.Patterns;

namespace EFTest.ModuleTest.LogicalOperationWithSpecificationTest.SpecCases;

// Спецификация 1: Возраст >= 40
public class IsActiveSpec : Specification<Employee>
{
    public IsActiveSpec() => AddFilteringQuery(e => e.Age >= 40);
}

// Спецификация 2: Возраст <= 45
public class IsManagerSpec : Specification<Employee>
{
    public IsManagerSpec() => AddFilteringQuery(e => e.Age <= 45);
}

// Спецификация 3: Возраст = 30 (для XOR)
public class IsJuniorSpec : Specification<Employee>
{
    public IsJuniorSpec() => AddFilteringQuery(e => e.Age == 30);
}