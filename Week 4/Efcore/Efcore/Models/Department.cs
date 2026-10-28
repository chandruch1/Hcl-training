using System;
using System.Collections.Generic;
using System.Text;

namespace Efcore.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentNmae { get; set; } = string.Empty;
        public string? Location { get; set; }

    }
}
