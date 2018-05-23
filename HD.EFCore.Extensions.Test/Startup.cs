using HD.EFCore.Extensions.Test.CacheItem;
using HD.EFCore.Extensions.Test.Data;
using HD.EFCore.Extensions.Test.Entity;
using HD.Host.Abstractors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HD.EFCore.Extensions.Test
{
    public class Startup
    {
        RedisClient _redis;
        MyDbContext _ctx;
        public Startup(MyDbContext ctx)
        {
            _redis = new RedisClient("192.168.8.6:6999,password=myredis,allowAdmin=true,connectTimeout=10000,syncTimeout=15000");
            _ctx = ctx;
            var blogs = _ctx.Blog.ToList();
        }
        /// <summary>
        /// 配置依赖注入
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddUnitOfWork();
            services.AddEntityCache(options =>
            {
                options.CachePrefix = "EFCoreExtensionsTest";
                options.Get = (type, key) =>
                {
                    string val = _redis.Database.StringGet(key);
                    return val?.FromJson(type);
                };
                options.Gets = (type, keys) =>
                {
                    var vals = _redis.Database.StringGet(keys.Select(q => (RedisKey)q)?.ToArray())?.Select(q => (string)q);
                    return vals?.Where(q => !string.IsNullOrWhiteSpace(q)).ToList().ConvertAll(q => q?.FromJson(type));
                };
                options.Set = (key, entity) =>
                {
                    if (entity == null) return false;
                    return _redis.Database.StringSet(key, entity.ToJson(), TimeSpan.FromHours(1));
                };
                options.Sets = dict =>
                {
                    if (dict == null) return false;

                    var kvs = dict.ToList().Select(q => new KeyValuePair<RedisKey, RedisValue>((RedisKey)q.Key, (RedisValue)q.Value.ToJson()));
                    var rst = _redis.Database.StringSet(kvs.ToArray());
                    kvs.Select(q => (string)q.Key).ToList().ForEach(key => _redis.Database.KeyExpire(key, TimeSpan.FromHours(1)));
                    return rst;
                };
                options.Del = key => _redis.Database.KeyDelete(key);

                options.Map = (entityType, entityVal) =>
                {
                    if (entityType == typeof(Blog))
                    {
                        var m = entityVal as Blog;
                        return new BlogItem { Id = m.Id, Title = m.Title, Body = m.Body };
                    }
                    return null;
                };
            });


            //自己扩展：使用DbContextPool的方式注入读写分离dbcontext
            services.AddDbContextPoolEnhance<MasterDbContext>(q => q.UseMySql<MasterDbContext>("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));
            services.AddDbContextPoolEnhance<SlaveDbContext>(q => q.UseMySql<SlaveDbContext>("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));

            //原生ef注入读写分离dbcontext的方式（缺点是不能使用DbContextPool的方式注入）
            //services.AddDbContext<MasterDbContext>(q => ((DbContextOptionsBuilder<MasterDbContext>)q).UseMySql<MasterHDDbContext>("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));
            //services.AddDbContext<SlaveDbContext>(q => ((DbContextOptionsBuilder<SlaveDbContext>)q).UseMySql<SlaveHDDbContext>("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));


            //
            services.AddSingleton<IHostedService, HostedService>();
            services.AddTransient<TestService, TestService>();
        }
    }
}
