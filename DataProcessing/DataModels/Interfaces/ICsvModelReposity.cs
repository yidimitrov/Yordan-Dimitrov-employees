using DataModels.Reflections.Csv;

namespace DataModels.Interfaces
{
    public interface ICsvModelReposity
    {
        CsvProperty[] GetModelProperties<TCsv>();
    }
}
