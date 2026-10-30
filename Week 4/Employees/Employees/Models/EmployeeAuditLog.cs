namespace Employees.Models
{
    public class EmployeeAuditLog
    {
        public int AuditId { get; set; }
        public int EmployeeId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}