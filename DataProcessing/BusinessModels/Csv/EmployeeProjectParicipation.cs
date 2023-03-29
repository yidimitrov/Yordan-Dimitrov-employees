namespace BusinessModels.Csv
{
    public class EmployeeProjectParicipation
    {
        public int EmpId { get; set; }

        public int ProjectId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public override string ToString()
        {
            return $"employee: {EmpId}, project: {ProjectId}, datefrom: {DateFrom}, dateto: {DateTo}";
        }
    }
}