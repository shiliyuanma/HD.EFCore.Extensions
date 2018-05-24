using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace HD.EFCore.Extensions.Cache
{
    internal class CacheHelper
    {
        /// <summary>
        /// key的缓存前缀
        /// </summary>
        public static EntityCacheOptions Options { get; set; }
        /// <summary>
        /// 内存缓存
        /// </summary>
        public static readonly ConcurrentDictionary<string, object> Cache = new ConcurrentDictionary<string, object>();
        /// <summary>
        /// db entity关联的cacheitem类型
        /// </summary>
        public static ConcurrentDictionary<Type, List<Type>> EntityMapCacheItems = new ConcurrentDictionary<Type, List<Type>>();

        /// <summary>
        /// 生成缓存key
        /// </summary>
        public static string GenKey<TEntity, TPrimaryKey>(TPrimaryKey key)
        {
            return $"{(!string.IsNullOrWhiteSpace(Options.CachePrefix) ? Options.CachePrefix + ":" : "")}EntityCache:{typeof(TEntity).Name}:{key}";
        }
        /// <summary>
        /// 生成缓存key
        /// </summary>
        public static string GenKey(Type entityType, object primaryKey)
        {
            return $"{(!string.IsNullOrWhiteSpace(Options.CachePrefix) ? Options.CachePrefix + ":" : "")}EntityCache:{entityType.Name}:{primaryKey.ToString()}";
        }
        /// <summary>
        /// 
        /// </summary>
        public static void Del(Type entityType, object primaryKey)
        {
            var keys = new List<string>();
            keys.Add(GenKey(entityType, primaryKey));

            if (EntityMapCacheItems.TryGetValue(entityType, out var cacheItemTypes))
            {
                foreach (var cacheItemType in cacheItemTypes)
                {
                    keys.Add(GenKey(cacheItemType, primaryKey));
                }
            }

            //删除相关的缓存
            foreach (var key in keys)
            {
                if (Options.Del != null)
                {
                    Options.Del(key);
                }
                else
                {
                    Cache.TryRemove(key, out var obj);
                }
            }
        }
    }
}
