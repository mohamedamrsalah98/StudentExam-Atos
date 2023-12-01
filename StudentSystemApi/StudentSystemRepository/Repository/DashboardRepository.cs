using Microsoft.EntityFrameworkCore;
using StudentSystemRepository.Interfaces;
using StudentSytemData.Data;
using StudentSytemData.Dto;
using StudentSytemData.Models;

namespace StudentSystemRepository.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DataContext _context;

        public DashboardRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<AllStudentAllSubjectDto>> GetStudentExamInfoAsync()
        {

            var result = await _context.ExamResults
                .Include(er => er.StudentExam.Student)
                .Include(er => er.StudentExam.Exam.Subject)
                .Select(er => new AllStudentAllSubjectDto
                {
                    StudentName = er.StudentExam.Student.UserName, 
                    SubjectName = er.StudentExam.Exam.Subject.SubjectName,
                    Score = er.Score,
                    DateTimeOfExam = er.ExamTime
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<ExamResult>> GetAllExamResultsAsync()
        {
            return await _context.ExamResults.ToListAsync();
        }
    }
}
