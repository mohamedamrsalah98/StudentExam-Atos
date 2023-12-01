using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Dto
{
    public class AllStudentAllSubjectDto
    {
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public int Score { get; set; }
        public DateTime DateTimeOfExam { get; set; }
    }
}
