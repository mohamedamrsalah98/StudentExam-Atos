using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Dto
{
    public class ExamSubmissionDto
    {


        public int ExamId { get; set; }
        public string StudentId { get; set; }
        public TimeSpan Duration { get; set; }
        public List<ChoiceSubmissionDto> Choices { get; set; }
        //public bool ExamSubmittedAutomatically { get; set; }

    }
}
