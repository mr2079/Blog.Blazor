using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Data.Extensions;

public static class ApplicationContextExtensions
{
    public static async Task InitializeDatabaseAsync(this IServiceCollection services)
    {
        var databaseInitializer = services.BuildServiceProvider()
            .GetRequiredService<DatabaseInitializer>();

        try
        {
            databaseInitializer.Initialize();
            await databaseInitializer.SeedRoleDataAsync();
            await databaseInitializer.SeedUserDataAsync();
        }
        catch
        {
	        // ignored
        }
    }
}
