namespace SkillHub.Models;

public class MentorReport
{
    public int Id { get; set; }
    public int MentorId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? Mentor { get; set; }
}
