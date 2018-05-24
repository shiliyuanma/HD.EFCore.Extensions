using Microsoft.Extensions.DependencyInjection.Extensions;
using HD.EFCore.Extensions.Cache;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CacheServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityCache(this IServiceCollection services, Action<EntityCacheOptions> cacheOptions)
        {
            services.Configure(cacheOptions);
            var options = new EntityCacheOptions();
            cacheOptions(options);
            if (options.Get != null && options.Set != null && options.Del != null)
            {
                services.TryAdd(ServiceDescriptor.Transient(typeof(IEntityStorage<,>), typeof(OutEntityStorage<,>)));
            }
            CacheHelper.Options = options;
            services.TryAdd(ServiceDescriptor.Transient(typeof(IEntityStorage<,>), typeof(MemoryEntityStorage<,>)));
            services.TryAdd(ServiceDescriptor.Transient(typeof(IEntityCache<,>), typeof(EntityCache<,>)));
            services.TryAdd(ServiceDescriptor.Transient(typeof(IEntityCache<,,>), typeof(EntityCache<,,>)));

            return services;
        }
    }
}
