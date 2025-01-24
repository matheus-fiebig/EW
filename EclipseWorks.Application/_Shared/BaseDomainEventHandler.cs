using EclipseWorks.Domain._Shared.Events;
using MediatR;

namespace EclipseWorks.Application._Shared
{
    public abstract class BaseDomainEventHandler<T> : INotificationHandler<T> where T : DomainEvent
    {
        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            await TryHandle(notification, cancellationToken);
        }

        public abstract Task TryHandle(T notification, CancellationToken cancellationToken);
    }
}
