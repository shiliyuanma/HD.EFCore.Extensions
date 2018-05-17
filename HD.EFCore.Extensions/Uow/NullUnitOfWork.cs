using Microsoft.EntityFrameworkCore.Storage;

namespace HD.EFCore.Extensions.Uow
{
    public class NullUnitOfWork : IUnitOfWork
    {
        MainUnitOfWork _mainUoW;
        public IDbContextTransaction Tran => _mainUoW.Tran;

        public NullUnitOfWork(IUnitOfWork mainUoW)
        {
            _mainUoW = mainUoW as MainUnitOfWork ?? throw new System.Exception("NullUnitOfWork需要一个MainUnitOfWork的参数");
        }

        public void Commit()
        {
        }
        public void Rollback()
        {
            _mainUoW.RollbackIncrement();
        }

        public void Dispose()
        {
        }
    }
}
