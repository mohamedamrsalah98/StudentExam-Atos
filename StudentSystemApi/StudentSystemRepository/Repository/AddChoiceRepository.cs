
using Microsoft.EntityFrameworkCore;
using StudentSystemRepository.Interfaces;
using StudentSytemData.Data;
using StudentSytemData.Models;

namespace StudentSystemRepository.Repository
{
    public class AddChoiceRepository : IAddChoiceRepository
    {
        private readonly DataContext _context;

        public AddChoiceRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<string> AddChoicesToQuestionAsync(Question question, Choice choice)
        {

            question.Choices.Add(choice);

            await _context.SaveChangesAsync();

            return string.Empty;
        }
        public async Task<bool> ChoiceExistsForQuestionAsync(Question question, string choiceText)
        {
            return await _context.Choices
                .AnyAsync(c => c.QuestionId == question.QuestionId && c.ChoiceText == choiceText);
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _context.Questions
                .Include(q => q.Choices)
                .FirstOrDefaultAsync(q => q.QuestionId == questionId);
        }
    }
}
