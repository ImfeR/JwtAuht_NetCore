namespace WebServiceAuthentication.Context;

using Microsoft.EntityFrameworkCore;
using Entities;

public class WebConfiguratorContext : DbContext
{
    public WebConfiguratorContext(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("SqLite");
    }

    public DbSet<User> Users { get; set; }

    private string ConnectionString { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(ConnectionString);
    }
}