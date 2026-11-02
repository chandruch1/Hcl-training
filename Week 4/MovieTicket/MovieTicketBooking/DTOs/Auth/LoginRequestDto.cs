using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Auth;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string PassWord { get; set; } = string.Empty;
}