using System;
using System.ComponentModel.DataAnnotations;

namespace Learning1.DTO
{
    public class CreateEmployeeDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Range(1,50000)]
        public Decimal? Salary { get; set; }
        public int? DepartmentId { get; set; }
    }
}
