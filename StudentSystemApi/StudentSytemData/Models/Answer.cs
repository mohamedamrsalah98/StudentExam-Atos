using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public bool RightChoice { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
