namespace BusinessModels.Csv
{
    public class LongestDaysWorkedEmployeesPair
    {
        public int EmployeeId1 { get; set; }

        public int EmployeeId2 { get; set; }

        public Dictionary<int, int> ProjectToDaysWorked { get; set; }
    }
}
