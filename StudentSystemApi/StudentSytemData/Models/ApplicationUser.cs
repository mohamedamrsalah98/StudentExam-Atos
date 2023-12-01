using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;
        public ICollection<StudentSubject> StudentSubjects { get; set; }
        public ICollection<StudentExam> StudentExams { get; set; }
    }
}
