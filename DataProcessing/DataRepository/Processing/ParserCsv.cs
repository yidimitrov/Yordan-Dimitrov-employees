using DataModels.Attributes;
using DataModels.Interfaces;
using DataModels.Reflections.Csv;
using DataRepository.Interfaces;

namespace DataRepository.Processing
{
    public class ParserCsv : IParsable
    {
        public ParserCsv(ICsvModelReposity csvModelReposity)
        {
            _csvModelRepository = csvModelReposity;
        }

        private readonly ICsvModelReposity _csvModelRepository;

        public IEnumerable<T> ParseCsvRows<T>(IEnumerable<string> csvrows)
            where T : class, new()
        {
            IEnumerable<T> models = csvrows.Select(l => ParseRow<T>(l))
                .Where(m => m != null);

            return models.ToArray();
        }

        private T ParseRow<T>(string csvline)
            where T : class, new()
        {
            if (string.IsNullOrEmpty(csvline.Trim()) || csvline.Trim().StartsWith("#"))
            {
                return default(T);
            }

            string[] csvfields = csvline.Split(',', '\t').Select(i => i.Trim()).ToArray();

            CsvProperty[] csvModelProperties = _csvModelRepository.GetModelProperties<T>();

            T model = BuildModel<T>(csvModelProperties, csvfields);

            return model;
        }

        private T BuildModel<T>(IEnumerable<CsvProperty> csvModelProperties, string[] csvfields)
            where T : class, new()
        {
            T model = new();

            foreach (CsvProperty property in csvModelProperties.OrderBy(p => p.Position))
            {
                if ((csvfields.Length <= property.Position || string.IsNullOrEmpty(csvfields[property.Position])) && property.Required)
                {
                    throw new InvalidDataException($"Invalid csv format - missing required field {property.Position}");
                }
                if (string.Compare(csvfields[property.Position],"NULL", true) == 0 && property.PossibleNull.Equals(ECsvFieldState.NotNullable))
                {
                    throw new InvalidDataException($"Invalid csv format - notnullable field is null {property.Position}");
                }

                var field = csvfields[property.Position];
                try
                {
                    if (string.Compare(csvfields[property.Position], "NULL", true) == 0)
                    {
                        if (new[] { typeof(int?), typeof(DateTime?), typeof(long), typeof(double) }.Contains(property.PropertyInfo.PropertyType))
                        {
                            property.PropertyInfo.SetValue(model, null);
                        }
                        else if (property.PropertyInfo.PropertyType == typeof(bool))
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else if (new[] { typeof(int), typeof(int?) }.Contains(property.PropertyInfo.PropertyType))
                    {
                        property.PropertyInfo.SetValue(model, int.Parse(field));
                    }
                    else if (new[] { typeof(DateTime), typeof(DateTime?) }.Contains(property.PropertyInfo.PropertyType))
                    {
                        property.PropertyInfo.SetValue(model, DateTime.Parse(field));
                    }
                    else if (property.PropertyInfo.PropertyType == typeof(string))
                    {
                        property.PropertyInfo.SetValue(model, field);
                    }
                    else if (new[] { typeof(long), typeof(long?) }.Contains(property.PropertyInfo.PropertyType))
                    {
                        property.PropertyInfo.SetValue(model, long.Parse(field));
                    }
                    else if (new[] { typeof(double), typeof(double?) }.Contains(property.PropertyInfo.PropertyType))
                    {
                        property.PropertyInfo.SetValue(model, double.Parse(field));
                    }
                    else if (new[] { typeof(bool), typeof(bool?) }.Contains(property.PropertyInfo.PropertyType))
                    {
                        throw new NotImplementedException();
                    }
                }
                catch (Exception exception)
                {
                    throw new InvalidDataException($"Invalid csv format field:{field}, type:{property.PropertyInfo.PropertyType.Name}");
                }
            }
            return model;
        }
    }
}
