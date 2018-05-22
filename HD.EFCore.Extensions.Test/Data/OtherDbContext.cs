using Microsoft.EntityFrameworkCore;

namespace HD.EFCore.Extensions.Test.Data
{
    public class MasterDbContext : MyDbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options)
        {
        }
    }

    public class SlaveDbContext : MyDbContext
    {
        public SlaveDbContext(DbContextOptions<SlaveDbContext> options) : base(options)
        {
        }
    }
}
