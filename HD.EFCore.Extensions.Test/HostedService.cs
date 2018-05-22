using HD.EFCore.Extensions.Test.Data;
using HD.Host.Abstractors;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HD.EFCore.Extensions.Test
{
    public class HostedService : IHostedService
    {
        MasterDbContext _masterDb;
        SlaveDbContext _slaveDb;
        UnitOfWorkService _uowSrv;
        public HostedService(MasterDbContext masterDb, SlaveDbContext slaveDb, UnitOfWorkService uowSrv)
        {
            _masterDb = masterDb;
            _slaveDb = slaveDb;
            _uowSrv = uowSrv;

            var blogs1 = _masterDb.Blog.ToList();
            var blogs2 = _slaveDb.Blog.ToList();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _uowSrv.SubTran();
            _uowSrv.Tran();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}
