using HD.EFCore.Extensions.Test.Data;
using HD.Host.Abstractors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace HD.EFCore.Extensions.Test
{
    public class Startup
    {
        MyDbContext _ctx;
        public Startup(MyDbContext ctx)
        {
            _ctx = ctx;
            var blogs = _ctx.Blog.ToList();
        }
        /// <summary>
        /// 配置依赖注入
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddUnitOfWork();

            services.AddSingleton<IHostedService, HostedService>();
            services.AddTransient<UnitOfWorkService, UnitOfWorkService>();

            //自己扩展：使用DbContextPool的方式注入读写分离dbcontext
            services.AddDbContextPoolEnhance<MasterDbContext>(q => q.UseMySql<MasterDbContext>("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));
            services.AddDbContextPoolEnhance<SlaveDbContext>(q => q.UseMySql<SlaveDbContext>("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));

            //原生ef注入读写分离dbcontext的方式（缺点是不能使用DbContextPool的方式注入）
            //services.AddDbContext<MasterDbContext>(q => ((DbContextOptionsBuilder<MasterDbContext>)q).UseMySql<MasterHDDbContext>("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));
            //services.AddDbContext<SlaveDbContext>(q => ((DbContextOptionsBuilder<SlaveDbContext>)q).UseMySql<SlaveHDDbContext>("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));
        }
    }
}
