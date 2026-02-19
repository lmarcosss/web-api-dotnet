using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Models;

namespace WebApi.Infra
{
  public class ConnectionContext : DbContext
  {
    public ConnectionContext(DbContextOptions<ConnectionContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
  }
}