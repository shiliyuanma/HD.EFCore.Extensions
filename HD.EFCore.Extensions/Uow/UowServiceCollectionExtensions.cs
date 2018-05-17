using HD.EFCore.Extensions.Uow;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UowServiceCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddSingleton<IUnitOfWorkAccessor, UnitOfWorkAccessor>();
            services.AddTransient<IUnitOfWorkManager, UnitOfWorkManager>();

            return services;
        }
    }
}
