using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HD.EFCore.Extensions.Cache
{
    public static class CacheDbContextExtensions
    {
        public static void PreSaveChangesForEntityCache(this DbContext db)
        {
            db.ChangeTracker.DetectChanges();
            var entrys = db.ChangeTracker.Entries().Where(dbEntityEntry => dbEntityEntry.State == EntityState.Modified || dbEntityEntry.State == EntityState.Deleted).ToList();
            foreach (var e in entrys)
            {
                var entityType = e.Metadata.ClrType;
                var keyName = e.Metadata.FindPrimaryKey().Properties.Select(x => x.Name).FirstOrDefault();
                var keyVal = entityType.GetProperty(keyName).GetValue(e.Entity);
                CacheHelper.Del(entityType, keyVal);
            }
        }
    }
}
