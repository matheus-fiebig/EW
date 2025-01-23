using EclipseWorks.Domain.Commentaries.Entities;

namespace EclipseWorks.Application.Commentaries.Models
{
    public class CommentaryQueryResponse
    {
        public string Description { get; init; }

        public DateTime CreatedAt { get; init; }

        public static CommentaryQueryResponse ToModel(CommentaryEntity e)
        {
            return new CommentaryQueryResponse()
            {
                Description = e.Description,
                CreatedAt = e.CreatedAt,
            };
        }

        public static IEnumerable<CommentaryQueryResponse> ToModel(List<CommentaryEntity> entities)
        {
            return entities.Select(ToModel);
        }
    }
}
