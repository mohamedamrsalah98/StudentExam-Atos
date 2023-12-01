using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Dto
{
    public class QuestionWithChoicesDto
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; }
        public List<ChoiceDto> Choices { get; set; }

    }
}
