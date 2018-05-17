using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace HD.EFCore.Extensions.Uow
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        IUnitOfWorkAccessor _uowAccessor;
        public UnitOfWorkManager(IUnitOfWorkAccessor uowAccessor)
        {
            _uowAccessor = uowAccessor;
        }

        public IUnitOfWork Begin(DbContext db)
        {
            return Begin(db, new UnitOfWorkOptions());
        }

        public IUnitOfWork Begin(DbContext db, TransactionScopeOption scopeOption)
        {
            return Begin(db, new UnitOfWorkOptions { Scope = scopeOption });
        }

        public IUnitOfWork Begin(DbContext db, UnitOfWorkOptions unitOfWorkOptions)
        {
            //todo unitOfWorkOptions process...

            if (_uowAccessor.UoW == null)
            {
                _uowAccessor.UoW = new MainUnitOfWork(db, _uowAccessor);
                return _uowAccessor.UoW;
            }
            else
            {
                return new NullUnitOfWork(_uowAccessor.UoW);
            }
        }
    }
}
