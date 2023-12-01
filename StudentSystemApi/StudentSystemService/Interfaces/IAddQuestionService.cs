using StudentSytemData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemService.Interfaces
{
    public interface IAddQuestionService
    {
        Task<string> AddQuestionToSubjectAsync(int subjectId, QuestionDto questionDto);
        Task<List<QuestionDto>> GetQuestionsForSubjectAsync(int subjectId);
        Task<List<QuestionDto>> GetAllQuestionsAsync();





    }
}
