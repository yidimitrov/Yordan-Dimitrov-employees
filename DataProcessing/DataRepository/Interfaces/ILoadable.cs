namespace DataRepository.Interfaces
{
    public interface ILoadable
    {
        IEnumerable<string> LoadCsv(string filename);
    }
}
