using System.Threading;

namespace HD.EFCore.Extensions.Uow
{
    public class UnitOfWorkAccessor : IUnitOfWorkAccessor
    {
        private static AsyncLocal<IUnitOfWork> _uow = new AsyncLocal<IUnitOfWork>();

        public IUnitOfWork UoW
        {
            get
            {
                return _uow.Value;
            }
            set
            {
                _uow.Value = value;
            }
        }
    }
}
