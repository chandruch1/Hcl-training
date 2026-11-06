using System.ComponentModel.DataAnnotations;
using CampusPlacement.Api.Enums;

namespace CampusPlacement.Api.Models;

public class College
{
    public int CollegeId { get; set; }

    [Required]
    [StringLength(200)]
    public string CollegeName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string CollegeCode { get; set; } = string.Empty;

    [StringLength(200)]
    public string? UniversityName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string OfficialEmail { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Address { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Website { get; set; }

    public CollegeStatus Status { get; set; }
        = CollegeStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}