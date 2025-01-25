namespace EclipseWorks.Domain._Shared.Models
{
    public class ValidationObject<TEntity>
    {
        public TEntity Entity { get; init; }
        public Issue Issue { get; init; }
        public bool HasIssue => Issue != null;

        public static implicit operator ValidationObject<TEntity>(Issue issue)
        {
            return new()
            {
                Issue = issue,
            };
        }

        public static implicit operator ValidationObject<TEntity>(TEntity left)
        {
            return new()
            {
                Entity = left,
            };
        }
    }
}
