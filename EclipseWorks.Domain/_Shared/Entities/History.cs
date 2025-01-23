using EclipseWorks.Domain._Shared.Enums;

namespace EclipseWorks.Domain._Shared.Entities
{
    public class History : Entity
    {
        public Guid OriginId { get; set; }
        
        public string TableName { get; set; }
        
        public string NewValue { get; set; }

        public EModificationType Type { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public virtual Entity Origin { get; set; }
    }
}
