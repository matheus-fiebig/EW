using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Enums;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleToAttribute("EclipseWorks.UnitTest")]
namespace EclipseWorks.Domain.Histories.Entities
{
    public class HistoryEntity : Entity
    {
        public string OriginTableName { get; init; }

        public string Changes { get; init; }

        public EModificationType Type { get; init; }

        public Guid CreatedBy { get; init; }

        protected HistoryEntity()
        {
            
        }

        public static HistoryEntity TryCreateNew(Guid? createdBy, string originTableName, object changes, EModificationType type = EModificationType.Created)
        {
            ArgumentNullException.ThrowIfNull(createdBy);
            ArgumentNullException.ThrowIfNullOrEmpty(originTableName);
            ArgumentNullException.ThrowIfNull(changes);

            JsonSerializerOptions jsonOpts = new JsonSerializerOptions()
            {
                MaxDepth = 0,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
            };

            return new()
            {
                CreatedBy = createdBy.Value,
                OriginTableName = originTableName,
                Type = type,
                CreatedAt = DateTime.Now,
                Changes = JsonSerializer.Serialize(changes, jsonOpts)
            };
        }
    }
}
