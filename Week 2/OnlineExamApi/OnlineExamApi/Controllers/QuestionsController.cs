using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExamApi.Data;
using OnlineExamApi.DTOs;
using OnlineExamApi.Models;

namespace OnlineExamApi.Controllers;

[ApiController]
[Route("api/questions")]
public class QuestionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public QuestionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuestion(
        CreateQuestionDto dto)
    {
        var question = new Question
        {
            Text = dto.Text.Trim(),
            Subject = dto.Subject.Trim(),
            OptionA = dto.OptionA.Trim(),
            OptionB = dto.OptionB.Trim(),
            OptionC = dto.OptionC.Trim(),
            OptionD = dto.OptionD.Trim(),
            CorrectOption = dto.CorrectOption.ToUpperInvariant()
        };

        if (string.IsNullOrWhiteSpace(question.Text) ||
            string.IsNullOrWhiteSpace(question.Subject) ||
            string.IsNullOrWhiteSpace(question.OptionA) ||
            string.IsNullOrWhiteSpace(question.OptionB) ||
            string.IsNullOrWhiteSpace(question.OptionC) ||
            string.IsNullOrWhiteSpace(question.OptionD))
        {
            return BadRequest(new
            {
                message = "All fields are required and cannot be blank."
            });
        }

        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetQuestionById),
            new { id = question.Id },
            question);
    }

    // GET: api/questions
    // GET: api/questions?subject=Java
    [HttpGet]
    public async Task<IActionResult> GetQuestions(
        [FromQuery] string? subject)
    {
        var query = _context.Questions.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(subject))
        {
            var searchSubject = subject.Trim();

            query = query.Where(q => q.Subject == searchSubject);
        }

        var questions = await query
            .OrderBy(q => q.Id)
            .ToListAsync();

        return Ok(questions);
    }

    // GET: api/questions/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetQuestionById(int id)
    {
        var question = await _context.Questions
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question == null)
        {
            return NotFound(new
            {
                message = $"Question with ID {id} was not found."
            });
        }

        return Ok(question);
    }

    // PUT: api/questions/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateQuestion(
        int id,
        UpdateQuestionDto dto)
    {
        var question = await _context.Questions
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question == null)
        {
            return NotFound(new
            {
                message = $"Question with ID {id} was not found."
            });
        }

        var text = dto.Text.Trim();
        var subject = dto.Subject.Trim();
        var optionA = dto.OptionA.Trim();
        var optionB = dto.OptionB.Trim();
        var optionC = dto.OptionC.Trim();
        var optionD = dto.OptionD.Trim();

        if (string.IsNullOrWhiteSpace(text) ||
            string.IsNullOrWhiteSpace(subject) ||
            string.IsNullOrWhiteSpace(optionA) ||
            string.IsNullOrWhiteSpace(optionB) ||
            string.IsNullOrWhiteSpace(optionC) ||
            string.IsNullOrWhiteSpace(optionD))
        {
            return BadRequest(new
            {
                message = "All fields are required and cannot be blank."
            });
        }

        question.Text = text;
        question.Subject = subject;
        question.OptionA = optionA;
        question.OptionB = optionB;
        question.OptionC = optionC;
        question.OptionD = optionD;
        question.CorrectOption = dto.CorrectOption.ToUpperInvariant();

        await _context.SaveChangesAsync();

        return Ok(question);
    }

    // DELETE: api/questions/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var question = await _context.Questions
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question == null)
        {
            return NotFound(new
            {
                message = $"Question with ID {id} was not found."
            });
        }

        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}