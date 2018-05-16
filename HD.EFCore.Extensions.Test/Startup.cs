using HD.Host.Abstractors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            services.AddSingleton<IHostedService, HostedService>();

            services.AddDbContextPoolEnhance<MasterHDDbContext>(q => q.UseMySql<MasterHDDbContext>("Server=192.168.4.127;Port=3306;Database=sso;Uid=root;Pwd=870224;"));
            services.AddDbContextPoolEnhance<SlaveHDDbContext>(q => q.UseMySql<SlaveHDDbContext>("Server=119.23.168.162;Port=53326;Database=fm_sso;Uid=betareader;Pwd=V6Wq8Z4mxihz;"));
        }
    }
}
