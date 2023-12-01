using StudentSytemData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemRepository.Interfaces
{
    public interface IAddChoiceRepository
    {
        Task<string> AddChoicesToQuestionAsync(Question question, Choice choice);
        Task<bool> ChoiceExistsForQuestionAsync(Question question, string choiceText);
        Task<Question> GetQuestionByIdAsync(int questionId);


    }
}
