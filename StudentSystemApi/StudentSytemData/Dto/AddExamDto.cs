using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StudentSytemData.Dto
{

        public class AddExamDto
        {
            [JsonIgnore]
             public int ExamId { get; set; }

            public string ExamTitle { get; set; }
            [JsonIgnore]

            public DateTime ExamDateTime { get; set; }
            public TimeSpan Duration { get; set; }
            public List<int> QuestionIds { get; set; }

    }

}
