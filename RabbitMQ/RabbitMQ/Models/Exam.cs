using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Models
{
    public class Exam
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public DateTime ExamDateTime { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public ICollection<StudentExam> StudentExams { get; set; }
        public ICollection<ExamQuestion> ExamQuestions { get; set; } 
    }
}
