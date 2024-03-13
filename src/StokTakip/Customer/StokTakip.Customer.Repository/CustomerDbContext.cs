using Microsoft.EntityFrameworkCore;
using StokTakip.Customer.Model;

namespace StokTakip.Customer.Repository;

public partial class CustomerDbContext(DbContextOptions<DbContext> options) : DbContext(options)
{
    public virtual DbSet<Model.Customer> Customers { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
}
