namespace EFTest.ModuleTest;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Employee> Employees { get; set; }

    public Company(string name)
    {
        Name = name;
    }
}