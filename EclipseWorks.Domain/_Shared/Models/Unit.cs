namespace EclipseWorks.Domain._Shared.Models
{
    public class Unit
    {
        public static Unit Value = new();

        private Unit()
        {

        }

        public static implicit operator bool(Unit _)
        {
            return true;
        }
    }
}
