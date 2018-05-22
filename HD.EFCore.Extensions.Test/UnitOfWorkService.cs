using HD.EFCore.Extensions.Test.Data;
using HD.EFCore.Extensions.Uow;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HD.EFCore.Extensions.Test
{
    public class UnitOfWorkService
    {
        IServiceProvider _sp;
        public UnitOfWorkService(IServiceProvider sp)
        {
            _sp = sp;
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
