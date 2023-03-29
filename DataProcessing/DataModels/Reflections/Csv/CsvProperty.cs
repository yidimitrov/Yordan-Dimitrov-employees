using DataModels.Attributes;
using System.Reflection;

namespace DataModels.Reflections.Csv
{
    public class CsvProperty
    {
        public int Position { get; set; }

        public ECsvFieldState PossibleNull { get; set; }

        public bool Required { get; set; }

        public PropertyInfo PropertyInfo { get; set; }
    }
}
