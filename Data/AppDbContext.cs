using Microsoft.EntityFrameworkCore;
using MyTodo.Models;

# pragma warning disable CS1591
namespace MyTodo.Data {
  using MyTodo.DotEnv;
  public class AppDbContext : DbContext {
    # pragma warning disable CS8618
    public DbSet<Todo> Todos {get; set;}
  
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      DotEnv.Load();
      var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
      var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
      var password = Environment.GetEnvironmentVariable("POSTGRES_PW");
      var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
      optionsBuilder.UseNpgsql($@"Host={host};Username={user};Password={password};Database={database}");
      // optionsBuilder.UseSqlite("DataSource=app.db;Cache=shared");
    }
  }
} 