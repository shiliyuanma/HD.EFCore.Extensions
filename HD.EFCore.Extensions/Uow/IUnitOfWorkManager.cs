using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace HD.EFCore.Extensions.Uow
{
    public interface IUnitOfWorkManager
    {
        IUnitOfWork Begin(DbContext db);

        IUnitOfWork Begin(DbContext db, TransactionScopeOption scopeOption);

        IUnitOfWork Begin(DbContext db, UnitOfWorkOptions unitOfWorkOptions);
    }
}
