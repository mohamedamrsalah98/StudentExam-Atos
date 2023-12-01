using Microsoft.EntityFrameworkCore;
using StudentSystemRepository.Interfaces;
using StudentSytemData.Data;
using StudentSytemData.Dto;
using StudentSytemData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemRepository.Repository
{
    public class AddQuestionRepository : IAddQuestionRepository
    {
        private readonly DataContext _context;

        public AddQuestionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Subject> GetSubjectByIdAsync(int subjectId)
        {
            var subject = await _context.Subjects
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.Id == subjectId);

            return subject;
        }
        public async Task<bool> QuestionExistsInSubjectAsync(Subject subject, string questionText)
        {
            return await _context.Questions
                .AnyAsync(q => q.SubjectId == subject.Id && q.QuestionText == questionText);
        }

        public async Task<string> AddQuestionsToSubjectAsync(Subject subject, Question question)
        {
            subject.Questions.Add(question);

            await _context.SaveChangesAsync();

            return string.Empty;
        }
        public async Task<List<QuestionDto>> GetQuestionsForSubjectAsync(int subjectId)
        {
            var subject = await _context.Subjects
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.Id == subjectId);

            var questions = subject?.Questions.ToList() ?? new List<Question>();

            // Assuming you have a QuestionDto class with the necessary properties
            var questionDtos = questions.Select(q => new QuestionDto
            {
                QuestionId = q.QuestionId,
                QuestionText = q.QuestionText,
                // Add other properties as needed
            }).ToList();

            return questionDtos;
        }

        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            return await _context.Questions.ToListAsync();
        }

    }
}
