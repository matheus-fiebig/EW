namespace EclipseWorks.Domain._Shared.Models
{
    public class Error
    {
        public string Code { get; init; }

        public string Description { get; init; }

        public static Error Create(string code, string description)
        {
            return new()
            {
                Description = description,
                Code = code
            };
        }
    }
}
