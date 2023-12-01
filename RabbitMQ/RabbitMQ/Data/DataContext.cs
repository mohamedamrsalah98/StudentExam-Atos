using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Models;


namespace StudentSytemData.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<StudentExam> StudentExams { get; set; }
        public DbSet<Answer> Answers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            builder.Entity<StudentSubject>()
                 .HasKey(ss =>  ss.StudentSubjectId);
            builder.Entity<StudentSubject>()
                .HasAlternateKey(ss => new { ss.StudentId, ss.SubjectId });
            builder.Entity<StudentSubject>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.StudentId);
            builder.Entity<StudentSubject>()
                .HasOne(ss => ss.Subject)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.SubjectId);


            builder.Entity<Exam>()
                .HasOne(e => e.Subject)
                .WithMany(s => s.Exams)
                .HasForeignKey(e => e.SubjectId);



            builder.Entity<Question>()
                .HasOne(q => q.Subject)
                .WithMany(s => s.Questions)
                .HasForeignKey(q => q.SubjectId);





            builder.Entity<ExamQuestion>()
                .HasKey(eq => eq.ExamQuestionId);
            builder.Entity<ExamQuestion>()
                .HasAlternateKey(eq => new { eq.ExamId, eq.QuestionId });
            builder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Exam)
                .WithMany(e => e.ExamQuestions)
                .HasForeignKey(eq => eq.ExamId);
            builder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Question)
                .WithMany(q => q.ExamQuestions)
                .HasForeignKey(eq => eq.QuestionId);




            builder.Entity<Choice>()
                .HasOne(c => c.Question)
                .WithMany(q => q.Choices)
                .HasForeignKey(c => c.QuestionId);


            builder.Entity<ExamResult>()
                .HasOne(er => er.StudentExam)
                .WithMany(se => se.ExamResults)
                .HasForeignKey(er => er.StudentExamId);





            builder.Entity<StudentExam>()
                .HasKey(se =>  se.StudentExamId);
            builder.Entity<StudentExam>()
                .HasAlternateKey(se => new { se.ExamId, se.StudentId });
            builder.Entity<StudentExam>()
                .HasMany(se => se.ExamResults)
                .WithOne(er => er.StudentExam)
                .HasForeignKey(er => er.StudentExamId)
                .HasPrincipalKey(se => se.StudentExamId);
            builder.Entity<StudentExam>()
                .HasOne(se => se.Exam)
                .WithMany(e => e.StudentExams)
                .HasForeignKey(se => se.ExamId);
            builder.Entity<StudentExam>()
                .HasOne(se => se.Student)
                .WithMany(s => s.StudentExams)
                .HasForeignKey(se => se.StudentId);


            builder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);
        }
    }
}
