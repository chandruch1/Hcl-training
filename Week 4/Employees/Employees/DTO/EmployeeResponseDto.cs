using Employees.Enums;

namespace Employees.DTO
{
    public class EmployeeResponseDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
    }
}