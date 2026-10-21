
namespace Hospital.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Reason { get; set; } = "";
        public string Status { get; set; } = "Scheduled";
    }
}
