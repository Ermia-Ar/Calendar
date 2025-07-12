using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore.Design;

public class AppDbContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=CalendarDatabase;Integrated Security=True;Trust Server Certificate=True");

        return new ApplicationContext(optionsBuilder.Options, 
            new FillBaseEntityValuesOnUpdatingInterceptor(), 
            new FillBaseEntityValuesOnCreatingInterceptor(),
            new SoftDeleteInterceptor());
    }
}

