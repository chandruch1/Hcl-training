namespace Employees.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; } = true;
        public Department? Department { get; set; }
    }
}
