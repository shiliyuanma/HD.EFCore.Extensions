using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace HD.EFCore.Extensions.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        IDbContextTransaction Tran { get; }

        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
    }
}
