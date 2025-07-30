using SkillHub.DTOs;

namespace SkillHub.Interfaces;

public interface IMentorReportService
{
    Task<List<MentorReportDto>> GetAllAsync();
    Task<MentorReportDto?> GetByIdAsync(int id);
    Task<MentorReportDto> CreateAsync(int mentorId, MentorReportCreateDto dto);
    Task<bool> UpdateAsync(int id, int mentorId, MentorReportUpdateDto dto);
    Task<bool> DeleteAsync(int id, int mentorId);
}
