using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Dto
{
    public class ExamQuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public IEnumerable<ChoiceDto> Choices { get; set; }
        public int StudentChoiceId { get; set; }
        public int CorrectChoiceId { get; set; }
    }
}
