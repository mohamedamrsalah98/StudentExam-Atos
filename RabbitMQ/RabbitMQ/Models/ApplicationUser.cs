using Microsoft.AspNetCore.Identity;


namespace RabbitMQ.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;
        public ICollection<StudentSubject> StudentSubjects { get; set; }
        public ICollection<StudentExam> StudentExams { get; set; }
    }
}
