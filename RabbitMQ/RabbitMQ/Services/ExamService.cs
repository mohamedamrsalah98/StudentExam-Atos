using Microsoft.EntityFrameworkCore;
using RabbitMQ.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using StudentSytemData.Data;
using RabbitMQ.Dto;
using RabbitMQ.Interfaces;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.SignaiR;

namespace RabbitMQ.Services
{
    public class ExamService : IExamService
    {
        private readonly DataContext _context;
        private readonly IHubContext<ExamHub> _hubContext;

        public ExamService(DataContext context, IHubContext<ExamHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public ExamResult EvaluateExamAndSave(ExamSubmissionDto examSubmission)
        { 
            if (examSubmission == null)
            {
                throw new ArgumentNullException(nameof(examSubmission), "ExamSubmissionDto cannot be null");
            }

            if (examSubmission.Choices == null || !examSubmission.Choices.Any())
            {
                throw new InvalidOperationException("ExamSubmissionDto must have at least one choice.");
            }

            var score = CalculateScore(examSubmission);
            var studentExam = _context.StudentExams.FirstOrDefault(se => se.ExamId == examSubmission.ExamId && se.StudentId == examSubmission.StudentId);


            if (studentExam == null)
            {
                throw new InvalidOperationException($"No StudentExam found for ExamId: {examSubmission.ExamId}");
            }

            var examResult = new ExamResult
            {
                StudentExamId = studentExam.StudentExamId, 
                Score = score,
                ExamTime = DateTime.Now
            };

            _context.ExamResults.Add(examResult);
            _context.SaveChanges();

            var userId = studentExam.StudentId;
            _hubContext.Clients.User(userId).SendAsync("ReceiveExamResult", score);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
            };

            string jsonString = JsonSerializer.Serialize(examResult, options);

            return examResult;
        }


        public int CalculateScore(ExamSubmissionDto examSubmission)
        {
            if (examSubmission == null)
            {
                throw new ArgumentNullException(nameof(examSubmission), "ExamSubmissionDto cannot be null");
            }

            if (examSubmission.Choices == null || !examSubmission.Choices.Any())
            {
                throw new InvalidOperationException("ExamSubmissionDto must have at least one choice.");
            }

            var totalChoices = examSubmission.Choices.Count;
            var correctChoices = examSubmission.Choices.Count(choice => IsChoiceCorrect(choice));

            if (totalChoices == 0)
            {
                throw new InvalidOperationException("ExamSubmissionDto must have at least one choice for scoring.");
            }

            var score = (int)(((double)correctChoices) / totalChoices * 100);
            return score;
        }
        public bool IsChoiceCorrect(ChoiceSubmissionDto choice)
        {
            if (choice == null)
            {
                throw new ArgumentNullException(nameof(choice), "ChoiceSubmissionDto cannot be null");
            }

            if (choice.SelectedChoiceId == 0)
            {
                return DefaultValueForEmptyChoice();
            }

            var correctChoice = _context.Choices.FirstOrDefault(c => c.ChoiceId == choice.SelectedChoiceId);

            if (correctChoice == null)
            {
                throw new InvalidOperationException($"No Choice found for ChoiceId: {choice.SelectedChoiceId}");
            }

            return correctChoice.IsCorrect;
        }

        private bool DefaultValueForEmptyChoice()
        {

            return false;
        }



    }
}
