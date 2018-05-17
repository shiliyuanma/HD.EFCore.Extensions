using Microsoft.EntityFrameworkCore.Storage;

namespace HD.EFCore.Extensions.Uow
{
    public class NullUnitOfWork : IUnitOfWork
    {
        public IDbContextTransaction Tran { get; private set; }

        public NullUnitOfWork(IDbContextTransaction tran)
        {
            Tran = tran;
        }

        public void Commit()
        {
        }
        public void Rollback()
        {
        }

        public void Dispose()
        {
        }
    }
}
