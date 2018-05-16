using HD.Host.Abstractors;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HD.EFCore.Extensions.Test
{
    public class HostedService : IHostedService
    {
        MasterHDDbContext _masterDb;
        SlaveHDDbContext _slaveDb;
        public HostedService(MasterHDDbContext masterDb, SlaveHDDbContext slaveDb)
        {
            _masterDb = masterDb;
            _slaveDb = slaveDb;

            var users1 = _masterDb.Aspnetusers.ToList();
            var users2 = _slaveDb.Aspnetusers.ToList();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}
