using Microsoft.EntityFrameworkCore;
using EFTest.ModuleTest;



public class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Company> Companies { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка отношения: Компания имеет много сотрудников
        modelBuilder.Entity<Company>()
            // много сотрудников
            .HasMany(c => c.Employees)
            // имеют одну компанию
            .WithOne(e => e.Company)
            // при удалении компании, удаляются сотрудники
            .OnDelete(DeleteBehavior.Cascade); // Опционально: каскадное удаление
        
        // Настройка для Employee
        modelBuilder.Entity<Employee>()
            .HasKey(e => e.Id);
    }
}