using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace HD.EFCore.Extensions.Cache
{
    public class OutEntityStorage<TEntity, TPrimaryKey> : IEntityStorage<TEntity, TPrimaryKey> where TEntity : class
    {
        EntityCacheOptions _options;
        public OutEntityStorage(IOptions<EntityCacheOptions> options)
        {
            _options = options.Value;
        }

        public TEntity Get(TPrimaryKey key)
        {
            return _options.Get(GenKey(key)) as TEntity;
        }

        public IEnumerable<TEntity> Gets(IEnumerable<TPrimaryKey> keys)
        {
            if ((keys?.Count() ?? 0) == 0)
            {
                return null;
            }

            var result = new List<TEntity>();
            if (_options.Gets != null)
            {
                result = _options.Gets(keys.Select(q => GenKey(q)))?.Select(q => q as TEntity).ToList();
            }
            else
            {
                foreach (var key in keys)
                {
                    var entity = Get(key);
                    if (entity != null)
                        result.Add(entity);
                }
            }
            return result?.Count > 0 ? result : null;
        }

        public bool Set(TPrimaryKey key, TEntity entity)
        {
            return _options.Set(GenKey(key), entity);
        }

        public bool Sets(Dictionary<TPrimaryKey, TEntity> entitys)
        {
            if (entitys == null || entitys.Count == 0)
            {
                return false;
            }

            if (_options.Sets != null)
            {
                return _options.Sets(entitys.Select(q => GenKey(q.Key)), entitys.Select(q => q.Value));
            }
            else
            {
                foreach (var item in entitys)
                {
                    Set(item.Key, item.Value);
                }
            }
            return true;
        }

        public bool Remove(TPrimaryKey key)
        {
            return _options.Del(GenKey(key));
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

    public class OutEntityStorage<TEntity, TPrimaryKey, TCacheItem> : OutEntityStorage<TEntity, TPrimaryKey>, IEntityStorage<TEntity, TPrimaryKey, TCacheItem> where TEntity : class where TCacheItem : class
    {
        EntityCacheOptions _options;
        public OutEntityStorage(IOptions<EntityCacheOptions> options) : base(options)
        {
            _options = options.Value;
        }

        public TCacheItem Map(TEntity entity)
        {
            return _options.Map != null ? _options.Map(typeof(TEntity), entity) as TCacheItem : default(TCacheItem);
        }
    }
}
