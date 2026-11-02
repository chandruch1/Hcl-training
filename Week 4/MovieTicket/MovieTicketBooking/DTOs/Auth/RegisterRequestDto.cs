using MovieTicketBooking.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Auth;

public class RegisterRequestDto
{

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
  
    
}