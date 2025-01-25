namespace EclipseWorks.Application.Projects.Models
{
    public sealed record CreateProjectRequest(string Name, string Description, List<Guid> ParticipantIds);
}
