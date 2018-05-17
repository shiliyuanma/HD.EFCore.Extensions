using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HD.EFCore.Extensions.Uow
{
    public class MainUnitOfWork : IUnitOfWork
    {
        DbContext _db;
        IDbContextTransaction _tran;
        IUnitOfWorkAccessor _uowAccessor;
        bool _isDisposed;

        public IDbContextTransaction Tran => _tran;

        public MainUnitOfWork(DbContext db, IUnitOfWorkAccessor uowAccessor)
        {
            _db = db;
            _tran = _db.Database.BeginTransaction();
            _uowAccessor = uowAccessor;
        }

        public void Commit()
        {
            _tran?.Commit();
        }

        public void Rollback()
        {
            _tran?.Rollback();
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _tran?.Dispose();
            _uowAccessor.UoW = null;

            _isDisposed = true;
        }
    }
}
