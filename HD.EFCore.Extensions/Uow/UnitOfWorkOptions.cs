using System;
using System.Transactions;

namespace HD.EFCore.Extensions.Uow
{
    /// <summary>
    /// Unit of work options.
    /// </summary>
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// Scope option.
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan? Timeout { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TransactionScopeAsyncFlowOption? AsyncFlowOption { get; set; }
    }
}