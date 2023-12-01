using StudentSytemData.Dto;
using StudentSytemData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemRepository.Interfaces
{
    public interface IAddQuestionRepository
    {
        Task<Subject> GetSubjectByIdAsync(int subjectId);
        Task<string> AddQuestionsToSubjectAsync(Subject subject, Question question);
        Task<bool> QuestionExistsInSubjectAsync(Subject subject, string questionText);
        Task<List<QuestionDto>> GetQuestionsForSubjectAsync(int subjectId);
        Task<List<Question>> GetAllQuestionsAsync();








    }
}
