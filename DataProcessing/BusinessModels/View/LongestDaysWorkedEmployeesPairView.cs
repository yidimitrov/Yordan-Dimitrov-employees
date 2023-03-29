using System.ComponentModel;

namespace BusinessModels.View
{
    public class LongestDaysWorkedEmployeesPairView
    {
        [DisplayName("Employee ID #1")]
        public int EmployeeId1 { get; set; }

        [DisplayName("Employee ID #2")]
        public int EmployeeId2 { get; set; }

        [DisplayName("Project ID")]
        public int ProjectId { get; set; }

        [DisplayName("Days worked")]
        public int DaysWorked { get; set; }
    }
}
