using EclipseWorks.Domain._Shared.Events;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EclipseWorks.Domain._Shared.Entities
{
    public class Entity
    {
        public Guid Id { get; init; }

        public DateTime CreatedAt { get; init; }


        [NotMapped]
        [JsonIgnore]
        public List<DomainEvent> Events { get; private set; } = new(); 

        public void AddEvent(DomainEvent e)
        {
            Events.Add(e);
        }

        public void ClearEvents()
        {
            Events.Clear();
        }
    }
}
