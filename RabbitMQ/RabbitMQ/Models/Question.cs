using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int SubjectId { get; set; }

        public Subject Subject { get; set; }
        public ICollection<Choice> Choices { get; set; }
        public ICollection<ExamQuestion> ExamQuestions { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
