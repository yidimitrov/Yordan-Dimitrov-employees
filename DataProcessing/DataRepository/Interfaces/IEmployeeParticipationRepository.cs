using BusinessModels.Csv;

namespace DataRepository.Interfaces
{
    public interface IEmployeeParticipationRepository
    {
        IEnumerable<EmployeeProjectParicipation> GetEmployeesData(IEnumerable<string> csvrows);
    }
}
