using EntityFrameworkCore.PrimaryKey;
using HD.EFCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HD.EFCore.Extensions.Cache
{
    public class EntityCache<TEntity, TPrimaryKey> : IEntityCache<TEntity, TPrimaryKey> where TEntity : class
    {
        IEntityStorage<TEntity, TPrimaryKey> _storage;
        public EntityCache(IEntityStorage<TEntity, TPrimaryKey> storage)
        {
            _storage = storage;
        }

        public TEntity Get(DbContext db, TPrimaryKey key, string keyName = "Id")
        {
            var entity = _storage.Get(key);
            if (entity != null)
            {
                return entity;
            }

            entity = db.Set<TEntity>().FirstOrDefault(ExpressionHelper.CreateEqualityExpressionForId<TEntity, TPrimaryKey>(key, keyName));
            if (entity != null)
            {
                _storage.Set(key, entity);
            }
            return entity;
        }

        public TEntity Get(DbContext db, TPrimaryKey key, Expression<Func<TEntity, bool>> expression)
        {
            var entity = _storage.Get(key);
            if (entity != null)
            {
                return entity;
            }

            entity = db.Set<TEntity>().FirstOrDefault(expression);
            if (entity != null)
            {
                _storage.Set(key, entity);
            }
            return entity;
        }

        public IEnumerable<TEntity> Gets(DbContext db, IEnumerable<TPrimaryKey> keys, string keyName = "Id")
        {
            if (keys == null || keys.Count() == 0) return null;

            var entitys = _storage.Gets(keys);
            if (entitys?.Count() == keys.Count())
            {
                return entitys;
            }

            entitys = db.Set<TEntity>().Where(ExpressionHelper.CreateContainsExpressionForId<TEntity, TPrimaryKey>(keys, keyName)).ToList();
            if (entitys != null && entitys.Count() > 0)
            {
                _storage.Sets(entitys.ToDictionary(k => (TPrimaryKey)(db.GetPrimaryKey(k)[keyName]), v => v));
            }
            return entitys;
        }

        public IEnumerable<TEntity> Gets(DbContext db, IEnumerable<TPrimaryKey> keys, Expression<Func<TEntity, bool>> expression)
        {
            if (keys == null || keys.Count() == 0) return null;

            var entitys = _storage.Gets(keys);
            if (entitys?.Count() == keys.Count())
            {
                return entitys;
            }

            entitys = db.Set<TEntity>().Where(expression).ToList();
            if (entitys != null && entitys.Count() > 0)
            {
                _storage.Sets(entitys.ToDictionary(k => (TPrimaryKey)(db.GetPrimaryKey(k).First().Value), v => v));
            }
            return entitys;
        }

        public bool Remove(TPrimaryKey key)
        {
            return _storage.Remove(key);
        }

        public bool Removes(IEnumerable<TPrimaryKey> keys)
        {
            return _storage.Removes(keys);
        }
    }

    public class EntityCache<TEntity, TPrimaryKey, TCacheItem> : IEntityCache<TEntity, TPrimaryKey, TCacheItem> where TEntity : class where TCacheItem : class
    {
        EntityCacheOptions _options;
        IEntityStorage<TCacheItem, TPrimaryKey> _storage;
        public EntityCache(IOptions<EntityCacheOptions> options, IEntityStorage<TCacheItem, TPrimaryKey> storage)
        {
            _options = options.Value;
            _storage = storage;
        }

        public TCacheItem Get(DbContext db, TPrimaryKey key, string keyName = "Id")
        {
            var cacheItem = _storage.Get(key);
            if (cacheItem != null)
            {
                return cacheItem;
            }

            var entity = db.Set<TEntity>().FirstOrDefault(ExpressionHelper.CreateEqualityExpressionForId<TEntity, TPrimaryKey>(key, keyName));
            if (entity != null)
            {
                cacheItem = Map(entity);
                if (cacheItem != null)
                    _storage.Set(key, cacheItem);
            }
            return cacheItem;
        }

        public TCacheItem Get(DbContext db, TPrimaryKey key, Expression<Func<TEntity, bool>> expression)
        {
            var cacheItem = _storage.Get(key);
            if (cacheItem != null)
            {
                return cacheItem;
            }

            var entity = db.Set<TEntity>().FirstOrDefault(expression);
            if (entity != null)
            {
                cacheItem = Map(entity);
                if (cacheItem != null)
                    _storage.Set(key, cacheItem);
            }
            return cacheItem;
        }

        public IEnumerable<TCacheItem> Gets(DbContext db, IEnumerable<TPrimaryKey> keys, string keyName = "Id")
        {
            if (keys == null || keys.Count() == 0) return null;

            var cacheItems = _storage.Gets(keys);
            if (cacheItems?.Count() == keys.Count())
            {
                return cacheItems;
            }

            var entitys = db.Set<TEntity>().Where(ExpressionHelper.CreateContainsExpressionForId<TEntity, TPrimaryKey>(keys, keyName)).ToList();
            if (entitys != null && entitys.Count() > 0)
            {
                var dict = entitys.ToDictionary(k => (TPrimaryKey)(db.GetPrimaryKey(k)[keyName]), v => Map(v));
                if (dict != null && dict.Count > 0 && dict.Any(q => q.Value != null))
                {
                    dict = dict.Where(q => q.Value != null).ToDictionary(k => k.Key, v => v.Value);
                    cacheItems = dict.Values;
                    _storage.Sets(dict);
                }
            }
            return cacheItems;
        }

        public IEnumerable<TCacheItem> Gets(DbContext db, IEnumerable<TPrimaryKey> keys, Expression<Func<TEntity, bool>> expression)
        {
            if (keys == null || keys.Count() == 0) return null;

            var cacheItems = _storage.Gets(keys);
            if (cacheItems?.Count() == keys.Count())
            {
                return cacheItems;
            }

            var entitys = db.Set<TEntity>().Where(expression).ToList();
            if (entitys != null && entitys.Count() > 0)
            {
                var dict = entitys.ToDictionary(k => (TPrimaryKey)(db.GetPrimaryKey(k).First().Value), v => Map(v));
                if (dict != null && dict.Count > 0 && dict.Any(q => q.Value != null))
                {
                    dict = dict.Where(q => q.Value != null).ToDictionary(k => k.Key, v => v.Value);
                    cacheItems = dict.Values;
                    _storage.Sets(dict);
                }
            }
            return cacheItems;
        }



        public bool Remove(TPrimaryKey key)
        {
            return _storage.Remove(key);
        }

        public bool Removes(IEnumerable<TPrimaryKey> keys)
        {
            return _storage.Removes(keys);
        }

        public TCacheItem Map(TEntity entity)
        {
            return _options.Map != null ? _options.Map(typeof(TEntity), entity) as TCacheItem : default(TCacheItem);
        }
    }
}
