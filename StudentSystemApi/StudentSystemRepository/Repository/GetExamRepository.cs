using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentSystemRepository.Interfaces;
using StudentSytemData.Data;
using StudentSytemData.Dto;
using StudentSytemData.Models;


namespace StudentSystemRepository.Repository
{
    public class GetExamRepository : IGetExamRepository
    {
        private readonly DataContext _context;




        public GetExamRepository( DataContext context)
        {
            _context = context;
        }

        public async Task<Exam> GetExamAsync(int subjectId)
        {

            //as no track read about it (Select) eng:ahmed ssamy 
            var exams = await _context.Exams
                .Where(e => e.SubjectId == subjectId)
                .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question)
                        .ThenInclude(q => q.Choices)
                .ToListAsync();

            var seed = DateTime.Now.Millisecond;

            var random = new Random();
            var randomExam = exams.OrderBy(e => random.Next()).FirstOrDefault();

            return randomExam;


        }

        public async Task<ExamSubmissionDto> SubmitExamAsync(int examId, string studentId, List<ChoiceSubmissionDto> selectedChoices)
        {
            try
            {
                var studentExam = new StudentExam
                {
                    ExamId = examId,
                    StudentId = studentId,
                    ExamResults = new List<ExamResult>()
                };

                _context.StudentExams.Add(studentExam);
                await _context.SaveChangesAsync();

                var examQuestions = await _context.ExamQuestions
                    .Where(eq => eq.ExamId == examId)
                    .Include(eq => eq.Question)
                    .ThenInclude(q => q.Choices)
                    .ToListAsync();

                var submittedChoices = new List<ChoiceSubmissionDto>();

                foreach (var examQuestion in examQuestions)
                {
                    var questionId = examQuestion.QuestionId;
                    var userSelectedChoiceId = selectedChoices
                        .Where(choice => choice.QuestionId == questionId)
                        .Select(choice => choice.SelectedChoiceId)
                        .FirstOrDefault();

                    // Set SelectedChoiceId to 0 if not chosen
                    var selectedChoiceId = userSelectedChoiceId != 0 ? userSelectedChoiceId : 0;

                    submittedChoices.Add(new ChoiceSubmissionDto
                    {
                        QuestionId = questionId,
                        SelectedChoiceId = selectedChoiceId
                    });
                }

                return new ExamSubmissionDto
                {
                    ExamId = examId,
                    StudentId = studentId,
                    Choices = submittedChoices
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ExamResultDto>> GetStudentExamsAsync(string studentId)
        {

            var examResults = await _context.ExamResults
                .Where(er => er.StudentExam.StudentId == studentId)
                .Select(er => new ExamResultDto
                {
                    ExamId = er.StudentExam.ExamId,
                    ExamTitle = er.StudentExam.Exam.ExamTitle,
                    SubjectTitle = er.StudentExam.Exam.Subject.SubjectName,
                    Score = er.Score,
                    ExamTime = er.ExamTime
                })
                .ToListAsync();

            return examResults;
        }
    }
    }

