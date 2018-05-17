namespace HD.EFCore.Extensions.Uow
{
    public interface IUnitOfWorkAccessor
    {
        IUnitOfWork UoW { get; set; }
    }
}
