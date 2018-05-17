using HD.Host.Abstractors;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HD.EFCore.Extensions.Test
{
    public class HostedService : IHostedService
    {
        MasterHDDbContext _masterDb;
        SlaveHDDbContext _slaveDb;
        UnitOfWorkService _uowSrv;
        public HostedService(MasterHDDbContext masterDb, SlaveHDDbContext slaveDb, UnitOfWorkService uowSrv)
        {
            _masterDb = masterDb;
            _slaveDb = slaveDb;
            _uowSrv = uowSrv;

            var users1 = _masterDb.Aspnetusers.ToList();
            var users2 = _slaveDb.Aspnetusers.ToList();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _uowSrv.Test();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}
