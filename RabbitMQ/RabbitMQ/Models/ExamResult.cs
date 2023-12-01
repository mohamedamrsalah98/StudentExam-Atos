using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Models
{
    public class ExamResult
    {
        public int ExamResultId { get; set; }
        public int StudentExamId { get; set; }
        public int Score { get; set; }
        public DateTime ExamTime { get; set; }
        public StudentExam StudentExam { get; set; }
    }
}
