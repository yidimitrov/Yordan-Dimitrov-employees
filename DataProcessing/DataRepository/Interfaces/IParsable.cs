namespace DataRepository.Interfaces
{
    public interface IParsable
    {
        IEnumerable<T> ParseCsvRows<T>(IEnumerable<string> csvrows) where T : class, new();
    }
}
