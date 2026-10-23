namespace CoursesWebApp.Models;

public class Learner
{
    public int LearnerId { get; set; }
    public string LearnerName { get; set; } = "";
    public string Email { get; set; } = "";
    public long Contact { get; set; }
}
