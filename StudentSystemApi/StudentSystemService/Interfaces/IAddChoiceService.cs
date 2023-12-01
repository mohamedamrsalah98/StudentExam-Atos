using StudentSytemData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemService.Interfaces
{
    public interface IAddChoiceService
    {
        Task<string> AddChoicesToQuestionAsync(int questionId, AddChoiceDto choiceDto);
        Task<List<AddChoiceDto>> GetChoicesForQuestionAsync(int questionId);


    }
}
