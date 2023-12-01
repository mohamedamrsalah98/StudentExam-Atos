using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Dto
{
    public class ExamResultDto
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public int Score { get; set; }
        public DateTime ExamTime { get; set; }
        public string SubjectTitle { get; set; }

    }
}
