namespace EclipseWorks.Domain._Shared.Models
{
    public class Issue
    {
        public string Code { get; init; }

        public string Description { get; init; }

        public static Issue CreateNew(string code, string description)
        {
            return new()
            {
                Description = description,
                Code = code
            };
        }
    }
}
