using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Events;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Infra.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EclipseWorks.Infra.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IPublisher _publisher;
        private readonly MainContext _context;

        public UnitOfWork(IPublisher publisher, MainContext context)
        {
            _publisher = publisher;
            _context = context;
        }

        private async Task PublishDomainEvents()
        {
            IEnumerable<EntityEntry<Entity>> entries = _context.ChangeTracker
                   .Entries<Entity>()
                   .Where(x => x.Entity.Events.Any());

            List<DomainEvent> events = entries
                .SelectMany(x => x.Entity.Events)
                .ToList();

            foreach (DomainEvent e in events)
            {
                await _publisher.Publish(e);
            }

            foreach (EntityEntry<Entity>? entry in entries)
            {
                entry.Entity.ClearEvents();
            }
        }

        public async Task BeginTrasactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await PublishDomainEvents();
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTrasactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
