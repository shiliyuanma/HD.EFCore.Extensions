using EntityFrameworkCore.PrimaryKey;
using Microsoft.EntityFrameworkCore;
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

            entity = db.Set<TEntity>().FirstOrDefault(CreateEqualityExpressionForId(key, keyName));
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
            var entitys = _storage.Gets(keys);
            if (entitys?.Count() > 0)
            {
                return entitys;
            }

            entitys = db.Set<TEntity>().Where(CreateContainsExpressionForId(keys, keyName)).ToList();
            if (entitys != null && entitys.Count() > 0)
            {
                _storage.Sets(entitys.ToDictionary(k => (TPrimaryKey)(db.GetPrimaryKey(k)[keyName]), v => v));
            }
            return entitys;
        }

        public IEnumerable<TEntity> Gets(DbContext db, IEnumerable<TPrimaryKey> keys, Expression<Func<TEntity, bool>> expression, string keyName = "Id")
        {
            var entitys = _storage.Gets(keys);
            if (entitys?.Count() > 0)
            {
                return entitys;
            }

            entitys = db.Set<TEntity>().Where(expression).ToList();
            if (entitys != null && entitys.Count() > 0)
            {
                _storage.Sets(entitys.ToDictionary(k => (TPrimaryKey)(db.GetPrimaryKey(k)[keyName]), v => v));
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

        private Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id, string keyName = "Id")
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, keyName),
                Expression.Constant(id, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        private Expression<Func<TEntity, bool>> CreateContainsExpressionForId(IEnumerable<TPrimaryKey> ids, string keyName = "Id")
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, keyName),
                Expression.Constant(keyName, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }

    public class EntityCache<TEntity, TPrimaryKey, TCacheItem> : IEntityCache<TEntity, TPrimaryKey, TCacheItem> where TEntity : class where TCacheItem : class
    {
        IEntityStorage<TEntity, TPrimaryKey, TCacheItem> _storage;
        public EntityCache(IEntityStorage<TEntity, TPrimaryKey, TCacheItem> storage)
        {
            _storage = storage;
        }

        public TCacheItem Get(DbContext db, TPrimaryKey key, string keyName = "Id")
        {
            var entity = _storage.Get(key);
            if (entity != null)
            {
                return _storage.Map(entity);
            }

            entity = db.Set<TEntity>().FirstOrDefault(CreateEqualityExpressionForId(key, keyName));
            if (entity != null)
            {
                _storage.Set(key, entity);
                return _storage.Map(entity);
            }
            return null;
        }

        public TCacheItem Get(DbContext db, TPrimaryKey key, Expression<Func<TEntity, bool>> expression)
        {
            var entity = _storage.Get(key);
            if (entity != null)
            {
                return _storage.Map(entity);
            }

            entity = db.Set<TEntity>().FirstOrDefault(expression);
            if (entity != null)
            {
                _storage.Set(key, entity);
                return _storage.Map(entity);
            }
            return null;
        }

        public IEnumerable<TCacheItem> Gets(DbContext db, IEnumerable<TPrimaryKey> keys, string keyName = "Id")
        {
            var entitys = _storage.Gets(keys);
            if (entitys?.Count() > 0)
            {
                return entitys.Select(q => _storage.Map(q));
            }

            entitys = db.Set<TEntity>().Where(CreateContainsExpressionForId(keys, keyName)).ToList();
            if (entitys != null && entitys.Count() > 0)
            {
                _storage.Sets(entitys.ToDictionary(k => (TPrimaryKey)(db.GetPrimaryKey(k)[keyName]), v => v));
                return entitys.Select(q => _storage.Map(q));
            }
            return null;
        }

        public IEnumerable<TCacheItem> Gets(DbContext db, IEnumerable<TPrimaryKey> keys, Expression<Func<TEntity, bool>> expression, string keyName = "Id")
        {
            var entitys = _storage.Gets(keys);
            if (entitys?.Count() > 0)
            {
                return entitys.Select(q => _storage.Map(q));
            }

            entitys = db.Set<TEntity>().Where(expression).ToList();
            if (entitys != null && entitys.Count() > 0)
            {
                _storage.Sets(entitys.ToDictionary(k => (TPrimaryKey)(db.GetPrimaryKey(k)[keyName]), v => v));
                return entitys.Select(q => _storage.Map(q));
            }
            return null;
        }

        public bool Remove(TPrimaryKey key)
        {
            return _storage.Remove(key);
        }

        public bool Removes(IEnumerable<TPrimaryKey> keys)
        {
            return _storage.Removes(keys);
        }

        private Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id, string keyName = "Id")
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, keyName),
                Expression.Constant(id, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        private Expression<Func<TEntity, bool>> CreateContainsExpressionForId(IEnumerable<TPrimaryKey> ids, string keyName = "Id")
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, keyName),
                Expression.Constant(keyName, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}
