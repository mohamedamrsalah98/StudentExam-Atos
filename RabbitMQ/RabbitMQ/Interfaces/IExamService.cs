using RabbitMQ.Dto;
using RabbitMQ.Models;

namespace RabbitMQ.Interfaces
{
    public interface IExamService
    {
        ExamResult EvaluateExamAndSave(ExamSubmissionDto examSubmission);

    }
}
