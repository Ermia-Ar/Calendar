using Infrastructure.Persistance.Data;
using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class AppIdentityDbContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=CalendarDb;Integrated Security=True;Trust Server Certificate=True");

        return new ApplicationContext(optionsBuilder.Options, 
            new FillBaseEntityValuesOnUpdatingInterceptor(), 
            new FillBaseEntityValuesOnCreatingInterceptor(),
            new SoftDeleteInterceptor());
    }
}

