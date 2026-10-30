using Employees.Enums;

namespace Employees.DTO
{
    public class InsertUserDto
    {
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public bool IsActive { get; set; } = true;
    }
}