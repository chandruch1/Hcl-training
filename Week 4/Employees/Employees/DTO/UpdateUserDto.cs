using Employees.Enums;

namespace Employees.DTO
{
    public class UpdateUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
    }
}