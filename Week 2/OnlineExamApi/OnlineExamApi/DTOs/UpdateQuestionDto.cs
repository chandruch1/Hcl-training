using System.ComponentModel.DataAnnotations;

namespace OnlineExamApi.DTOs;

public class UpdateQuestionDto
{
    [Required]
    public string Text { get; set; } = string.Empty;

    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string OptionA { get; set; } = string.Empty;

    [Required]
    public string OptionB { get; set; } = string.Empty;

    [Required]
    public string OptionC { get; set; } = string.Empty;

    [Required]
    public string OptionD { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^[A-Da-d]$",
        ErrorMessage = "CorrectOption must be A, B, C, or D.")]
    public string CorrectOption { get; set; } = string.Empty;
}