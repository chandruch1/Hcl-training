namespace StudentApi.Models;

public class AppSettings
{
    public string AppName { get; set; } = "";
    public string College { get; set; } = "";
    public int MaxStudents { get; set; }
    public string Environment { get; set; } = "";
}