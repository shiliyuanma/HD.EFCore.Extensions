using HD.Host.Abstractors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace HD.EFCore.Extensions.Test
{
    public class Startup
    {
        HDDbContext _ctx;
        public Startup(HDDbContext ctx)
        {
            _ctx = ctx;
            var users = _ctx.Aspnetusers.ToList();
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
            services.AddDbContextPoolEnhance<MasterHDDbContext>(q => q.UseMySql<MasterHDDbContext>("Server=192.168.4.127;Port=3306;Database=sso;Uid=root;Pwd=870224;"));
            services.AddDbContextPoolEnhance<SlaveHDDbContext>(q => q.UseMySql<SlaveHDDbContext>("Server=119.23.168.162;Port=53326;Database=fm_sso;Uid=betareader;Pwd=V6Wq8Z4mxihz;"));

            //原生ef注入读写分离dbcontext的方式（缺点是不能使用DbContextPool的方式注入）
            //services.AddDbContext<MasterHDDbContext>(q => ((DbContextOptionsBuilder<MasterHDDbContext>)q).UseMySql<MasterHDDbContext>("Server=192.168.4.127;Port=3306;Database=sso;Uid=root;Pwd=870224;"));
            //services.AddDbContext<SlaveHDDbContext>(q => ((DbContextOptionsBuilder<SlaveHDDbContext>)q).UseMySql<SlaveHDDbContext>("Server=119.23.168.162;Port=53326;Database=fm_sso;Uid=betareader;Pwd=V6Wq8Z4mxihz;"));
        }
    }
}
