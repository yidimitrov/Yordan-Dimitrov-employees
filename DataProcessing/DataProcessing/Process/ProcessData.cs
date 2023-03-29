using BusinessModels.Csv;
using BusinessModels.View;
using DataProcessing.Interfaces;
using DataRepository.Interfaces;

namespace DataProcessing.Process
{
    public class ProcessData : IService
    {
        public ProcessData(ILoadable loadable, IEmployeeParticipationRepository employeeParticipationRepository)
        {

            _loadable = loadable;
            _employeeParticipationRepository = employeeParticipationRepository;
        }

        private readonly ILoadable _loadable;
        private readonly IEmployeeParticipationRepository _employeeParticipationRepository;

        public IEnumerable<LongestDaysWorkedEmployeesPairView> GetLongestEmployeesParticipation(string csvfile)
        {
            try
            {
                string[] csvrows = _loadable.LoadCsv(csvfile).ToArray();

                IEnumerable<EmployeeProjectParicipation> employees = _employeeParticipationRepository.GetEmployeesData(csvrows);

                LongestDaysWorkedEmployeesPair longestPraticipation = ProcessCsvData(employees);

                return longestPraticipation.ProjectToDaysWorked.Select(e => new LongestDaysWorkedEmployeesPairView
                {
                    EmployeeId1 = longestPraticipation.EmployeeId1,
                    EmployeeId2 = longestPraticipation.EmployeeId2,
                    ProjectId = e.Key,
                    DaysWorked = e.Value
                });
            }
            catch (Exception exception)
            {

            }
            return Enumerable.Empty<LongestDaysWorkedEmployeesPairView>();
        }

        private LongestDaysWorkedEmployeesPair ProcessCsvData(IEnumerable<EmployeeProjectParicipation> employees)
        {

            // Grouping employees by projects
            var query = employees.GroupBy(
                emp => emp.ProjectId,
                (project, employee) => new
                {
                    Key = project,
                    Count = employee.Count(),
                    Employee = employee
                }).Where(e => e.Count > 1);


            Func<DateTime, DateTime, DateTime> Max = (d1,d2) => DateTime.Compare(d1, d2) < 0 ? d2 : d1;
            Func<DateTime, DateTime, DateTime> Min = (d1, d2) => DateTime.Compare(d1, d2) > 0 ? d2 : d1;
            
            List<EmployeesPair> employeesCommonProjects = new();
            // Compare project collaboration for each pair of employees. Calculate days they worked together
            foreach (IEnumerable<EmployeeProjectParicipation>? empProject in query.Select(g => g.Employee))
            {
                for (int i = 0; i < empProject.Count() - 1; i++)
                {
                    var emp1 = empProject.ElementAt(i);
                    for (int n = i + 1; n < empProject.Count(); n++)
                    {
                        var emp2 = empProject.ElementAt(n);
                        var currentDuration = Min(emp1.DateTo, emp2.DateTo) - Max(emp1.DateFrom, emp2.DateFrom);
                        if (currentDuration > TimeSpan.Zero)
                        {
                            employeesCommonProjects.Add(new EmployeesPair
                            {
                                ProjectId = emp1.ProjectId,
                                EmployeeId1 = Math.Min(emp1.EmpId, emp2.EmpId),
                                EmployeeId2 = Math.Max(emp1.EmpId, emp2.EmpId),
                                DaysWorked = currentDuration.Days
                            });
                        }
                    }
                }
            }

            // Grouping by pair employees
            var pairs = from gr in employeesCommonProjects
                          group gr by new { gr.EmployeeId1, gr.EmployeeId2 } into prj
                          select
                          new
                          {
                              Employee1 = prj.Key.EmployeeId1,
                              Employee2 = prj.Key.EmployeeId2,
                              Count = prj.Count(),
                              Duration = prj.Sum(p => p.DaysWorked),
                              Projects = prj.Select(e => new { Project = e.ProjectId, DaysWorked = e.DaysWorked })
                          };

            if (!pairs.Any())
            {
                return new LongestDaysWorkedEmployeesPair
                {
                    EmployeeId1 = -1,
                    EmployeeId2 = -1,
                    ProjectToDaysWorked = new Dictionary<int, int> { }
                };
            }

            var longestPair = pairs.First();

            return new LongestDaysWorkedEmployeesPair
            {
                EmployeeId1 = longestPair.Employee1,
                EmployeeId2 = longestPair.Employee2,
                ProjectToDaysWorked = longestPair.Projects.OrderByDescending(p => p.DaysWorked).ToDictionary(k => k.Project, v => v.DaysWorked)
            };
        }
    }
}
