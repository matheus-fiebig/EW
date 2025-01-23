using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Infra.Data.Context;

namespace EclipseWorks.Infra.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MainContext _context;

        public UnitOfWork(MainContext context)
        {
            _context = context;
        }

        public async Task BeginTrasactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTrasactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
