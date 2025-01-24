using EclipseWorks.Application._Shared;
using EclipseWorks.Domain.Histories.Entities;
using EclipseWorks.Domain.Histories.Events;
using EclipseWorks.Domain.Histories.Interfaces;

namespace EclipseWorks.Application.Histories.Handlers
{
    public class AddHistoryDomainEventHandler : BaseDomainEventHandler<AddHistoryDomainEvent>
    {
        private readonly ICommandHistoryRepository _commandHistoryRepository;

        public AddHistoryDomainEventHandler(ICommandHistoryRepository commandHistoryRepository)
        {
            _commandHistoryRepository = commandHistoryRepository;
        }

        public override async Task TryHandle(AddHistoryDomainEvent notification, CancellationToken cancellationToken)
        {
            var history = HistoryEntity.TryCreateNew(notification.CreatedBy,
                                                     notification.OriginTableName,
                                                     notification.Changes,
                                                     notification.Type);

            await _commandHistoryRepository.InsertAsync(history, cancellationToken);
        }
    }
}
