using DataModels.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Csv
{
    [CsvData]
    public class CsvEmployeeProjectParicipation
    {
        [CsvField(0)]
        [Required]
        public int EmpId { get; set; }

        [CsvField(1)]
        [Required]
        public int ProjectId { get; set; }

        [CsvField(2)]
        [Required]
        public DateTime DateFrom { get; set; }

        [CsvField(3, ECsvFieldState.Nullable)]
        [Required]
        public DateTime? DateTo { get; set; }
    }
}
