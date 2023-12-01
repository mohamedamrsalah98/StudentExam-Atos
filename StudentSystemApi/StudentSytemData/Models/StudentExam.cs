using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Models
{
    public class StudentExam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int StudentExamId { get; set; }
        public int ExamId { get; set; }
        public string StudentId { get; set; }

        public ApplicationUser Student { get; set; }
        public Exam Exam { get; set; }
        public ICollection<ExamResult> ExamResults { get; set; }
    }
}
