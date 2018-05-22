using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace HD.EFCore.Extensions.Cache
{
    public class MemoryEntityStorage<TEntity, TPrimaryKey> : IEntityStorage<TEntity, TPrimaryKey> where TEntity : class
    {
        static readonly ConcurrentDictionary<string, object> cache = new ConcurrentDictionary<string, object>();

        public MemoryEntityStorage()
        {
        }

        public TEntity Get(TPrimaryKey key)
        {
            if (cache.TryGetValue(GenKey(key), out var entity))
            {
                return entity as TEntity;
            }
            return null;
        }

        public IEnumerable<TEntity> Gets(IEnumerable<TPrimaryKey> keys)
        {
            if ((keys?.Count() ?? 0) == 0)
            {
                return null;
            }

            var result = new List<TEntity>();
            foreach (var key in keys)
            {
                var entity = Get(key);
                if (entity != null)
                    result.Add(entity);
            }
            return result.Count > 0 ? result : null;
        }

        public bool Set(TPrimaryKey key, TEntity entity)
        {
            return cache.TryAdd(GenKey(key), entity);
        }

        public bool Sets(Dictionary<TPrimaryKey, TEntity> entitys)
        {
            if (entitys == null || entitys.Count == 0)
            {
                return false;
            }

            foreach (var item in entitys)
            {
                Set(item.Key, item.Value);
            }
            return true;
        }

        public bool Remove(TPrimaryKey key)
        {
            return cache.TryRemove(GenKey(key), out var obj);
        }

        public bool Removes(IEnumerable<TPrimaryKey> keys)
        {
            if (keys == null || keys.Count() == 0)
            {
                return false;
            }

            foreach (var key in keys)
            {
                Remove(key);
            }
            return true;
        }

        private string GenKey(TPrimaryKey key)
        {
            return $"Entity:{typeof(TEntity).Name}:{key}";
        }
    }

    public class MemoryEntityStorage<TEntity, TPrimaryKey, TCacheItem> : MemoryEntityStorage<TEntity, TPrimaryKey>, IEntityStorage<TEntity, TPrimaryKey, TCacheItem> where TEntity : class where TCacheItem : class
    {
        public TCacheItem Map(TEntity entity)
        {
            return default(TCacheItem);
        }
    }
}
