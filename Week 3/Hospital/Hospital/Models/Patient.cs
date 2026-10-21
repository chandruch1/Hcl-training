namespace Hospital.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = "";
        public int Age { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string? Email { get; set; }
    }
}