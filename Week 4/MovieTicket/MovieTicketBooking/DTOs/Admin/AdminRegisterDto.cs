using MovieTicketBooking.Models;

namespace MovieTicketBooking.DTOs.Admin
{
    public class AdminRegisterDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; }=string.Empty
        public Role role { get; set; }
    }
}
