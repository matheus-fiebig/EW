using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain._Shared.Events;

namespace EclipseWorks.Domain.Histories.Events
{
    public sealed record AddHistoryDomainEvent(string OriginTableName, Guid CreatedBy, object Changes, EModificationType Type) : DomainEvent;
}
