using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Dto
{
    public class StudentSubjectDto
    {

        public int SubjectId { get; set; }
        public string StudentId { get; set; }
    }
}
