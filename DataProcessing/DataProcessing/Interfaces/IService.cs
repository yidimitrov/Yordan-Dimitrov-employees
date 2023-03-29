using BusinessModels.View;

namespace DataProcessing.Interfaces
{
    public interface IService
    {
        IEnumerable<LongestDaysWorkedEmployeesPairView> GetLongestEmployeesParticipation(string csvfile);
    }
}
