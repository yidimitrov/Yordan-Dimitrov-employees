namespace DataModels.Attributes
{
    public enum ECsvFieldState
    {
        NotNullable,
        Nullable,
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CsvFieldAttribute : Attribute
    {
        public CsvFieldAttribute(int position, ECsvFieldState possibleNull = ECsvFieldState.NotNullable)
        {
            Position = position;
            PossibleNull = possibleNull;
        }

        public int Position { get; set; }

        public ECsvFieldState PossibleNull  { get; set; }
    }
}
