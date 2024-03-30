//using Marasem.Application.Common.Exceptions;
//using Marasem.Application.Contracts.Data;
//using Microsoft.Extensions.DependencyInjection;

//namespace Marasem.Infrastructure.Data.Extensions;

//public static class ApplicationContextExtensions
//{
//    public static async Task InitializeDatabaseAsync(this IServiceCollection services)
//    {
//        var databaseInitializer = services.BuildServiceProvider()
//            .GetRequiredService<IDatabaseInitializer>();

//        try
//        {
//            databaseInitializer.Initialize();
//            // await databaseInitializer.SeedRoleDataAsync();
//            // await databaseInitializer.SeedUserDataAsync();
//        }
//        catch (Exception ex)
//        {
//            throw new ContextException("User|Role", ex);
//        }
//    }
//}
