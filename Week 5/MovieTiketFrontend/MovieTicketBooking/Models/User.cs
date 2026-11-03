
using Microsoft.AspNetCore.Identity;

namespace MovieTicketBooking.Models;

public class User
{
    public int UserId{get;set;}
    public string UserName { get; set; } =string.Empty;
    public string HashPassword {get;set;}=string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive {get;set;}=true;
    public DateTime CreatedAt {get;set;}= DateTime.UtcNow;
    public Role role { get; set; }
}