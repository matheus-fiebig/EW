namespace EclipseWorks.Application.Commentaries.Models
{
    public sealed record AddCommentaryRequest(string Commentary, Guid LoggedUserId);
}
