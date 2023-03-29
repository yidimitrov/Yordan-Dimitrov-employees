namespace BusinessModels.Csv
{
    public class EmployeesPair
    {
        public int EmployeeId1 { get; set; }

        public int EmployeeId2 { get; set; }

        public int ProjectId { get; set; }

        public int DaysWorked { get; set; }

        public override string ToString()
        {
            return $"employee1: {EmployeeId1}, employee2: {EmployeeId2}, project: {ProjectId}, daysworked: {DaysWorked}";
        }
    }
}