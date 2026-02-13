using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Infra
{
  public class ConnectionContext : DbContext
  {
    public ConnectionContext(DbContextOptions<ConnectionContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
  }
}