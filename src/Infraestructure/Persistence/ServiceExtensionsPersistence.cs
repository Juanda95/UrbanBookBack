using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repository;
using Persistence.Repository.Interface;
using Persistence.UnitOfWork;
using Persistence.UnitOfWork.Interface;

namespace Persistence
{
    public static class ServiceExtensionsPersistence
    {
        public static void AddPersistenceServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UrbanBookDbContext>(options =>
               options.UseNpgsql(connectionString, npgOptions =>
               {
                   npgOptions.MigrationsAssembly("Persistence");
                   npgOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
               })
               .ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning)));

            #region Repositories

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, ConfUnitOfWork>();
            #endregion Repositories


        }

    }
}
