﻿using HD.EFCore.Extensions.Cache;
using HD.EFCore.Extensions.Test.CacheItem;
using HD.EFCore.Extensions.Test.Data;
using HD.EFCore.Extensions.Test.Entity;
using HD.EFCore.Extensions.Uow;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

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
                var cache1 = scope.ServiceProvider.GetService<IEntityCache<Blog, int>>();
                var cache2 = scope.ServiceProvider.GetService<IEntityCache<Blog, int, BlogItem>>();
                var keys = new List<int> { 1, 2 };

                var m1 = cache1.Get(db, 1);
                var m2 = cache1.Gets(db, keys);


                var mm1 = cache2.Gets(db, keys)?.ToList();
                var mm2 = cache2.Gets(db, keys, q => keys.Contains(q.Id))?.ToList();

                var b = new Blog
                {
                    Title = "ttt",
                    Body = "body",
                    UserId = 100,
                    CreateTime = DateTime.Now
                };
                db.Blog.Add(b);
                db.SaveChanges();
                db.Blog.Remove(b);
                db.SaveChanges();

                b = db.Blog.FirstOrDefault(q => q.Id == 1);
                db.Blog.Remove(b);
                db.SaveChanges();
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
