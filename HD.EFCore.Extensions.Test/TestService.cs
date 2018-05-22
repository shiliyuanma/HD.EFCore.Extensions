using HD.EFCore.Extensions.Cache;
using HD.EFCore.Extensions.Test.Cache;
using HD.EFCore.Extensions.Test.Data;
using HD.EFCore.Extensions.Test.Entity;
using HD.EFCore.Extensions.Uow;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HD.EFCore.Extensions.Test
{
    public class TestService
    {
        IServiceProvider _sp;
        public TestService(IServiceProvider sp)
        {
            _sp = sp;
        }

        public void TestCache()
        {
            using (var scope = _sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<MasterDbContext>();
                var cache = scope.ServiceProvider.GetService<IEntityCache<Blog, int, BlogItem>>();

                var m = cache.Get(db, 1);

            }
        }

        public void Tran()
        {
            using (var scope = _sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<MasterDbContext>();
                var uowMgr = scope.ServiceProvider.GetService<IUnitOfWorkManager>();

                using (var uow = uowMgr.Begin(db))
                {
                    try
                    {

                        

                        SubTran(scope);

                        uow.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        uow.Rollback();
                    }
                }
            }
        }

        public void SubTran(IServiceScope scope = null)
        {
            scope = scope ?? _sp.CreateScope();
            var db = scope.ServiceProvider.GetService<MasterDbContext>();
            var uowMgr = scope.ServiceProvider.GetService<IUnitOfWorkManager>();

            using (var uow = uowMgr.Begin(db))
            {
                try
                {
                    throw new Exception("测试回滚");

                    db.SaveChanges();

                    uow.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    uow.Rollback();
                }
            }
        }
    }
}
