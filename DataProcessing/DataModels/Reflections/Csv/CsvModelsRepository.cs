using DataModels.Attributes;
using DataModels.Interfaces;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Reflections.Csv
{
    using ReflectedCollection = Dictionary<Type, CsvProperty[]>;
    public class CsvModelsRepository : ICsvModelReposity
    {
        public CsvModelsRepository()
        {
            _reflectedCollection = new();
        }

        private readonly ReflectedCollection _reflectedCollection;

        public CsvProperty[] GetModelProperties<TCsv>()
        {
            if (!_reflectedCollection.ContainsKey(typeof(TCsv)))
            {
                IEnumerable<CsvProperty> properties = ReflectCsvModels<TCsv>();
                ValidateModel(properties);
                _reflectedCollection[typeof(TCsv)] = properties.ToArray();
            }
            return _reflectedCollection[typeof(TCsv)];
        }

        private IEnumerable<CsvProperty> ReflectCsvModels<TCsv>()
        {
            Type model = Assembly.GetExecutingAssembly().GetTypes()
                .Single(t => t.IsClass && t.Equals(typeof(TCsv)) && t.GetCustomAttribute(typeof(CsvDataAttribute)) != null);

            T getAttribute<T>(PropertyInfo propertyInfo) where T : Attribute=>
                propertyInfo.GetCustomAttribute<T>();

            IEnumerable<CsvProperty> reflecedModel =
                model.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(property => property.GetCustomAttribute<CsvFieldAttribute>() != null)
                .Select(property =>
                new CsvProperty
                {
                    Position = getAttribute<CsvFieldAttribute>(property).Position,
                    PossibleNull = getAttribute<CsvFieldAttribute>(property).PossibleNull,
                    PropertyInfo = property,
                    Required = getAttribute<RequiredAttribute>(property) != null
                });

            return reflecedModel;
        }

        private void ValidateModel(IEnumerable<CsvProperty> properties)
        {
            foreach (var prop in properties)
            {
                bool defIsNullable = Nullable.GetUnderlyingType(prop.PropertyInfo.PropertyType) != null;

                if (prop.PossibleNull.Equals(ECsvFieldState.Nullable) &&! defIsNullable)
                {
                    throw new FormatException($"Inconsistent format model property {prop}");
                }
            }
        }
    }
}
