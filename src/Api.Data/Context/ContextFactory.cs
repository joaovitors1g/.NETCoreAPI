using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Data.Context
{
  public class ContextFactory : IDesignTimeDbContextFactory<MyContext>
  {
    public MyContext CreateDbContext(string[] args)
    {
      string connectionString = "Server=localhost,Port=3306,Database=DotnetAPI,Uid=root,Pwd=123";

      var optionsBuilder = new DbContextOptionsBuilder<MyContext>();

      optionsBuilder.UseMySql(connectionString);

      return new MyContext(optionsBuilder.Options);
    }
  }
}
