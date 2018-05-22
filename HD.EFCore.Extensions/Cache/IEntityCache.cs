using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HD.EFCore.Extensions.Cache
{
    public interface IEntityCache<TEntity, TPrimaryKey> where TEntity : class 
    {
        TEntity Get(DbContext db, TPrimaryKey key, string keyName = "Id");
        TEntity Get(DbContext db, TPrimaryKey key, Expression<Func<TEntity, bool>> expression);

        IEnumerable<TEntity> Gets(DbContext db, IEnumerable<TPrimaryKey> keys, string keyName = "Id");
        IEnumerable<TEntity> Gets(DbContext db, IEnumerable<TPrimaryKey> keys, Expression<Func<TEntity, bool>> expression);


        bool Remove(TPrimaryKey key);
        bool Removes(IEnumerable<TPrimaryKey> keys);
    }

    public interface IEntityCache<TEntity, TPrimaryKey, TCacheItem> where TEntity : class where TCacheItem : class
    {
        TCacheItem Get(DbContext db, TPrimaryKey key, string keyName = "Id");
        TCacheItem Get(DbContext db, TPrimaryKey key, Expression<Func<TEntity, bool>> expression);

        IEnumerable<TCacheItem> Gets(DbContext db, IEnumerable<TPrimaryKey> keys, string keyName = "Id");
        IEnumerable<TCacheItem> Gets(DbContext db, IEnumerable<TPrimaryKey> keys, Expression<Func<TEntity, bool>> expression);


        bool Remove(TPrimaryKey key);
        bool Removes(IEnumerable<TPrimaryKey> keys);
    }
}
