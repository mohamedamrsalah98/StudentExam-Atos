using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Dto
{
    public class GetExamDto
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public DateTime ExamDateTime { get; set; }
        public TimeSpan Duration { get; set; }

        public ICollection<QuestionWithChoicesDto> Questions { get; set; }

    }

}
