namespace EclipseWorks.Domain._Shared.Interfaces.UOW
{
    public interface IUnitOfWork
    {
        Task BeginTrasactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTrasactionAsync();
    }
}
