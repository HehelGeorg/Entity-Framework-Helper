namespace EFTest.ModuleTest;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    public int Age { get; set; }
    
    public Company Company { get; set; }

    public Employee(string name, string email, int age)
    {
        Name = name;
        Email = email;
        Age = age;
    }
}