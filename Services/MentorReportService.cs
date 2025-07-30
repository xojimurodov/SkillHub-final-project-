using Microsoft.EntityFrameworkCore;
using SkillHub.Data;
using SkillHub.DTOs;
using SkillHub.Interfaces;
using SkillHub.Models;

namespace SkillHub.Services;

public class MentorReportService : IMentorReportService
{
    private readonly SkillHubDbContext _context;

    public MentorReportService(SkillHubDbContext context)
    {
        _context = context;
    }

    public async Task<List<MentorReportDto>> GetAllAsync()
    {
        return await _context.MentorReports
            .Select(r => new MentorReportDto
            {
                Id = r.Id,
                MentorId = r.MentorId,
                Title = r.Title,
                Content = r.Content,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<MentorReportDto?> GetByIdAsync(int id)
    {
        var report = await _context.MentorReports.FindAsync(id);
        if (report == null) return null;

        return new MentorReportDto
        {
            Id = report.Id,
            MentorId = report.MentorId,
            Title = report.Title,
            Content = report.Content,
            CreatedAt = report.CreatedAt
        };
    }

    public async Task<MentorReportDto> CreateAsync(int mentorId, MentorReportCreateDto dto)
    {
        var report = new MentorReport
        {
            MentorId = mentorId,
            Title = dto.Title,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow
        };

        _context.MentorReports.Add(report);
        await _context.SaveChangesAsync();

        return new MentorReportDto
        {
            Id = report.Id,
            MentorId = report.MentorId,
            Title = report.Title,
            Content = report.Content,
            CreatedAt = report.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(int id, int mentorId, MentorReportUpdateDto dto)
    {
        var report = await _context.MentorReports.FirstOrDefaultAsync(r => r.Id == id && r.MentorId == mentorId);
        if (report == null) return false;

        report.Title = dto.Title;
        report.Content = dto.Content;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id, int mentorId)
    {
        var report = await _context.MentorReports.FirstOrDefaultAsync(r => r.Id == id && r.MentorId == mentorId);
        if (report == null) return false;

        _context.MentorReports.Remove(report);
        await _context.SaveChangesAsync();
        return true;
    }
}
