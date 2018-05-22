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
        TestService _testSrv;
        public HostedService(MasterDbContext masterDb, SlaveDbContext slaveDb, TestService uowSrv)
        {
            _masterDb = masterDb;
            _slaveDb = slaveDb;
            _testSrv = uowSrv;

            var blogs1 = _masterDb.Blog.ToList();
            var blogs2 = _slaveDb.Blog.ToList();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _testSrv.TestCache();
            _testSrv.SubTran();
            _testSrv.Tran();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}
