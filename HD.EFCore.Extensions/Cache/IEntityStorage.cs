using System.Collections.Generic;

namespace HD.EFCore.Extensions.Cache
{
    public interface IEntityStorage<TEntity, TPrimaryKey> where TEntity : class 
    {
        TEntity Get(TPrimaryKey key);

        IEnumerable<TEntity> Gets(IEnumerable<TPrimaryKey> keys);

        bool Set(TPrimaryKey key, TEntity entity);

        bool Sets(Dictionary<TPrimaryKey, TEntity> entitys);

        bool Remove(TPrimaryKey key);

        bool Removes(IEnumerable<TPrimaryKey> keys);
    }
}
