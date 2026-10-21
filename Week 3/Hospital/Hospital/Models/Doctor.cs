using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; } = "";
        public string Specalization { get; set; } = "";
        public decimal CounsultFee { get; set; }
    }
}
