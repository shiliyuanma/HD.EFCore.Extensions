using Microsoft.Extensions.DependencyInjection.Extensions;
using HD.EFCore.Extensions.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CacheServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityCache(this IServiceCollection services)
        {
            services.TryAdd(ServiceDescriptor.Transient(typeof(IEntityStorage<,>), typeof(MemoryEntityStorage<,>)));
            services.TryAdd(ServiceDescriptor.Transient(typeof(IEntityCache<,>), typeof(EntityCache<,>)));
            return services;
        }
    }
}
