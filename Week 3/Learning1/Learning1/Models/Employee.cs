using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Learning1.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal? Salary { get; set; }

        [Column("Department")]
        public string? ExistingDepartment { get; set; }

        public int? DepartmentId { get; set; }

        [JsonIgnore]
        public Department? DepartmentInfo { get; set; }
    }
}