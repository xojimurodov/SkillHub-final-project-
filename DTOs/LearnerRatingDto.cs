namespace SkillHub.DTOs;

public class LearnerRatingDto
{
    public int LearnerId { get; set; }
    public string LearnerName { get; set; } = string.Empty;
    public double AverageGrade { get; set; }
    public int TotalGrades { get; set; }
}
