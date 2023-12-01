using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Models
{
    public class StudentSubject
    {


        public int StudentSubjectId { get; set; }
        public string StudentId { get; set; }
        public int SubjectId { get; set; }
        public ApplicationUser Student { get; set; }
        public Subject Subject { get; set; }
    }
}
