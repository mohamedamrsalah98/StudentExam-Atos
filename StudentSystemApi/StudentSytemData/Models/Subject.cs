using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public ICollection<StudentSubject> StudentSubjects { get; set; }
        public ICollection<Exam> Exams { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
