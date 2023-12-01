using Microsoft.EntityFrameworkCore;
using StudentSystemRepository.Interfaces;
using StudentSytemData.Data;
using StudentSytemData.Dto;
using StudentSytemData.Models;

namespace StudentSystemRepository.Repository
{
    public class AddExamRepository : IAddExamRepository
    {
        private readonly DataContext _context;

        public AddExamRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddExamAsync(int subjectId, AddExamDto examDto)
        {
            var exam = new Exam
            {
                ExamTitle = examDto.ExamTitle,
                ExamDateTime = DateTime.Now,
                Duration = examDto.Duration,
                SubjectId = subjectId,

            }; 

            // Retrieve the selected questions by their IDs
            var selectedQuestions = _context.Questions
                .Where(q => examDto.QuestionIds.Contains(q.QuestionId))
                .ToList();

            // Map the selected questions to the exam
            exam.ExamQuestions = selectedQuestions.Select(q => new ExamQuestion
            {
                Question = q
            }).ToList();

            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();
            

        }
    }
}
