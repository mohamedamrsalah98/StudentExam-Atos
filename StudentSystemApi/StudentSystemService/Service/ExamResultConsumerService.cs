

using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using StudentSystemService.RabbitMQ;
using StudentSytemData.Data;
using StudentSytemData.Dto;
using StudentSytemData.Models;
using System.Text;

namespace StudentSystemService.Service
{
    public class ExamResultConsumerService : BackgroundService
    {
        private readonly DataContext _context;
        private readonly IRabbitMQProducer _rabbitMQService;
        //private readonly IExamResultRepository _examResultRepository; // Add your repository interface

        public ExamResultConsumerService(IRabbitMQProducer rabbitMQService,DataContext context)
        {
            _rabbitMQService = rabbitMQService;
            //_examResultRepository = examResultRepository;
            _context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var channel = _rabbitMQService.Connect())
            {
                channel.QueueDeclare(queue: "Exam", durable: false, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        var examSubmission = JsonConvert.DeserializeObject<ExamSubmissionDto>(message);

                        var examResult = EvaluateExamAndSave(examSubmission);

                        //_examResultRepository.SaveExamResult(examResult);

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                };
                if (channel != null && channel.IsOpen)
                {
                    channel.BasicConsume(queue: "Exam", autoAck: false, consumerTag: null, noLocal: false, exclusive: false, arguments: null, consumer: consumer);
                }
                await Task.CompletedTask;
            }
        }

        private ExamResult EvaluateExamAndSave(ExamSubmissionDto examSubmission)
        {
            var score = CalculateScore(examSubmission);

            var studentExam = _context.StudentExams.FirstOrDefault(se => se.StudentExamId == examSubmission.ExamId);

            // Create a new ExamResult entity
            var examResult = new ExamResult
            {
                StudentExamId = studentExam?.StudentExamId ?? 0, // Use the actual identifier
                Score = score,
                ExamTime = DateTime.Now
            };

            _context.ExamResults.Add(examResult);
            _context.SaveChanges();

            return examResult;
        }

        private int CalculateScore(ExamSubmissionDto examSubmission)
        {
            var totalChoices = examSubmission.Choices.Count;
            var correctChoices = examSubmission.Choices.Count(choice => IsChoiceCorrect(choice));
            var score = (int)(((double)correctChoices) / totalChoices * 100);
            return score;
        }

        private bool IsChoiceCorrect(ChoiceSubmissionDto choice)
        {
            var correctChoice = _context.Choices.FirstOrDefault(c => c.ChoiceId == choice.SelectedChoiceId);
            return correctChoice?.IsCorrect ?? false;
        }
    }
}

//    public class ExamResultConsumerService : BackgroundService
//    {
//        private readonly ResultEvaluationService _resultEvaluationService;
//        private readonly RabbitMQConfig _config;

//        public ExamResultConsumerService(IRABBITMQService rabbitMQService, IOptions<RabbitMQConfig> config, ResultEvaluationService resultEvaluationService)
//        {
//            _rabbitMQService = rabbitMQService;
//            _resultEvaluationService = resultEvaluationService;
//            _config = config.Value;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            using (var channel = _rabbitMQService.Connect())
//            {
//                channel.QueueDeclare(queue: _config.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

//                var consumer = new EventingBasicConsumer(channel);
//                consumer.Received += async (model, ea) =>
//                {
//                    var body = ea.Body.ToArray();
//                    var message = Encoding.UTF8.GetString(body);

//                    // Deserialize the received message to ExamSubmissionDto
//                    var examSubmission = JsonConvert.DeserializeObject<ExamSubmissionDto>(message);

//                    // Process the received message (ExamSubmissionDto) and save the exam result
//                    _resultEvaluationService.EvaluateAndSaveExamResult(examSubmission);

//                    // Acknowledge the message
//                    channel.BasicAck(ea.DeliveryTag, false);
//                };

//                channel.BasicConsume(queue: _config.QueueName, autoAck: false, consumer: consumer);

//                await Task.CompletedTask;
//            }
//        }
//    }

//    //public class HelloWorldConsumerService : BackgroundService
//    //{
//    //    private readonly IRabbitMQService _rabbitMQService;

//    //    public HelloWorldConsumerService(IRabbitMQService rabbitMQService)
//    //    {
//    //        _rabbitMQService = rabbitMQService;
//    //    }

//    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    //    {
//    //        while (!stoppingToken.IsCancellationRequested)
//    //        {
//    //            // Consume messages and print them
//    //            var message = await _rabbitMQService.ConsumeMessage();
//    //            Console.WriteLine($"Received message: {message}");

//    //            // Simulate some work
//    //            await Task.Delay(1000);
//    //        }
//    //    }
//    //}

//    //public class ExamResultConsumerService : BackgroundService
//    //{
//    //    private readonly IRabbitMQService _rabbitMQService;
//    //    private readonly RabbitMQConfig _config;
//    //    private readonly DataContext _context;



//    //    public ExamResultConsumerService(IRabbitMQService rabbitMQService, IOptions<RabbitMQConfig> config)
//    //    {
//    //        _rabbitMQService = rabbitMQService;
//    //        _config = config.Value;
//    //    }

//    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    //    {
//    //        using (var channel = _rabbitMQService.Connect())
//    //        {
//    //            channel.QueueDeclare(queue: _config.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

//    //            var consumer = new EventingBasicConsumer(channel);
//    //            consumer.Received += async (model, ea) =>
//    //            {
//    //                var body = ea.Body.ToArray();
//    //                var message = Encoding.UTF8.GetString(body);

//    //                var examSubmission = JsonConvert.DeserializeObject<ExamSubmissionDto>(message);

//    //                // Your existing logic to evaluate the exam and save the result
//    //                var evaluationResult = EvaluateExam(examSubmission);

//    //                var examResultDto = new ExamResultDto
//    //                {
//    //                    StudentExamId = evaluationResult.StudentExamId,
//    //                    Score = evaluationResult.Score,
//    //                    ExamTime = evaluationResult.ExamTime
//    //                };

//    //                // Save the result or perform further processing as needed
//    //                // _examResultService.SaveExamResult(examResultDto);

//    //                channel.BasicAck(ea.DeliveryTag, false);
//    //            };

//    //            channel.BasicConsume(queue: _config.QueueName, autoAck: false, consumer: consumer);

//    //            await Task.CompletedTask;
//    //        }
//    //    }

//    //    // Your existing methods for evaluating and processing exam results
//    //    private ExamResult EvaluateExam(ExamSubmissionDto examSubmission)
//    //    {
//    //        // Implement your logic to evaluate the exam and calculate the score
//    //        // ...

//    //        // For example:
//    //        var score = CalculateScore(examSubmission);

//    //        return new ExamResult
//    //        {
//    //            StudentExamId = examSubmission.StudentId,
//    //            Score = score,
//    //            ExamTime = DateTime.Now
//    //        };
//    //    }

//    //    private int CalculateScore(ExamSubmissionDto examSubmission)
//    //    {
//    //        // Implement your logic to calculate the score
//    //        // ...

//    //        // For example:
//    //        var totalChoices = examSubmission.Choices.Count;
//    //        var correctChoices = examSubmission.Choices.Count(choice => IsChoiceCorrect(choice));
//    //        var score = (int)(((double)correctChoices) / totalChoices * 100);

//    //        return score;
//    //    }

//    //    private bool IsChoiceCorrect(ChoiceSubmissionDto choice)
//    //    {
//    //        // Implement your logic to check if the choice is correct
//    //        // ...
//    //         var correctChoice = _context.Choices.FirstOrDefault(c => c.ChoiceId == choice.SelectedChoiceId);
//    //         return correctChoice?.IsCorrect ?? false;

//    //    }
//    //}
//    // ExamResultConsumerService.cs
//    //public class ExamResultConsumerService : BackgroundService
//    //{
//    //    private readonly IRabbitMQService _rabbitMQService;
//    //    private readonly RabbitMQConfig _config;

//    //    private readonly ExamResultService _examResultService;
//    //    private readonly DataContext _context;


//    //    public ExamResultConsumerService(IRabbitMQService rabbitMQService, ExamResultService examResultService, DataContext context, IOptions<RabbitMQConfig> config)
//    //    {
//    //        _rabbitMQService = rabbitMQService;
//    //        _examResultService = examResultService;
//    //        _context = context;
//    //        _config = config.Value;

//    //    }


//    //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    //{
//    //    var factory = new ConnectionFactory() { HostName = "localhost" };
//    //    using var connection = factory.CreateConnection();
//    //    using var channel = connection.CreateModel();

//    //    channel.QueueDeclare(queue: "exam-submissions", durable: false, exclusive: false, autoDelete: false, arguments: null);

//    //    var consumer = new EventingBasicConsumer(channel);
//    //    consumer.Received += (model, ea) =>
//    //    {
//    //        var body = ea.Body.ToArray();
//    //        var message = Encoding.UTF8.GetString(body);

//    //        // Deserialize the message to ExamSubmissionDto
//    //        var examSubmission = JsonConvert.DeserializeObject<ExamSubmissionDto>(message);

//    //        // Process the submitted exam, evaluate the result, and save the result
//    //        var evaluationResult = EvaluateExam(examSubmission);

//    //        // Save the result in the ExamResult table
//    //        _examResultService.SaveExamResult(evaluationResult);

//    //        // Acknowledge the message
//    //        channel.BasicAck(ea.DeliveryTag, false);
//    //    };

//    //    channel.BasicConsume(queue: "exam-submissions", autoAck: false, consumer: consumer);

//    //    await Task.CompletedTask;
//    //}

//    //    private ExamResult EvaluateExam(ExamSubmissionDto examSubmission)
//    //    {
//    //        // Implement your logic to evaluate the exam and calculate the score
//    //        // ...

//    //        // For example:
//    //        var score = CalculateScore(examSubmission);

//    //        return new ExamResult
//    //        {
//    //            StudentExamId = examSubmission.StudentId,
//    //            Score = score,
//    //            ExamTime = DateTime.Now
//    //        };
//    //    }

//    //    private int CalculateScore(ExamSubmissionDto examSubmission)
//    //    {
//    //        // Implement your logic to calculate the score
//    //        // ...

//    //        // For example:
//    //        var totalChoices = examSubmission.Choices.Count;
//    //        var correctChoices = examSubmission.Choices.Count(choice => IsChoiceCorrect(choice));
//    //        var score = (int)(((double)correctChoices) / totalChoices * 100);

//    //        return score;
//    //    }

//    //    private bool IsChoiceCorrect(ChoiceSubmissionDto choice)
//    //    {
//    //        // Implement your logic to determine if the choice is correct based on the database
//    //        // ...

//    //        // For example:
//    //        var correctChoice = _context.Choices.FirstOrDefault(c => c.ChoiceId == choice.SelectedChoiceId);
//    //        return correctChoice?.IsCorrect ?? false;
//    //    }
//    //}

//    //public class ExamResultConsumerService : BackgroundService
//    //{
//    //    private readonly IRabbitMQService _rabbitMQService;
//    //    private readonly ExamResultRepository _examResultService;
//    //    private readonly DataContext _context;

//    //    public ExamResultConsumerService(IRabbitMQService rabbitMQService, ExamResultRepository examResultService, DataContext context)
//    //    {
//    //        _rabbitMQService = rabbitMQService;
//    //        _examResultService = examResultService;
//    //        _context = context;
//    //    }

//    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    //    {
//    //        var factory = new ConnectionFactory() { HostName = "localhost" };
//    //        using var connection = factory.CreateConnection();
//    //        using var channel = connection.CreateModel();

//    //        channel.QueueDeclare(queue: "exam-submissions", durable: false, exclusive: false, autoDelete: false, arguments: null);

//    //        var consumer = new EventingBasicConsumer(channel);
//    //        consumer.Received += (model, ea) =>
//    //        {
//    //            var body = ea.Body.ToArray();
//    //            var message = Encoding.UTF8.GetString(body);

//    //            // Deserialize the message to ExamSubmissionDto
//    //            var examSubmission = JsonConvert.DeserializeObject<ExamSubmissionDto>(message);

//    //            // Process the submitted exam, evaluate the result, and save the result
//    //            var evaluationResult = EvaluateExam(examSubmission);

//    //            // Convert the evaluation result to ExamResultDto
//    //            var examResultDto = new ExamResultDto
//    //            {
//    //                StudentExamId = evaluationResult.StudentExamId,
//    //                Score = evaluationResult.Score,
//    //                ExamTime = evaluationResult.ExamTime
//    //            };

//    //            // Save the result in the ExamResult table
//    //            _examResultService.SaveExamResult(examResultDto);

//    //            // Acknowledge the message
//    //            channel.BasicAck(ea.DeliveryTag, false);
//    //        };

//    //        channel.BasicConsume(queue: "exam-submissions", autoAck: false, consumer: consumer);

//    //        await Task.CompletedTask;
//    //    }

//    //    public ExamResultDto EvaluateExam( ExamSubmissionDto examSubmission)
//    //    {
//    //        // Implement your logic to evaluate the exam and calculate the score
//    //        // ...

//    //        // For example:
//    //        var score = CalculateScore(examSubmission);

//    //        return new ExamResultDto
//    //        {
//    //            StudentExamId = examSubmission.StudentId,
//    //            Score = score,
//    //            ExamTime = DateTime.Now
//    //        };
//    //    }
//    //    private int CalculateScore(ExamSubmissionDto examSubmission)
//    //    {
//    //        // Implement your logic to calculate the score
//    //        // ...

//    //        // For example:
//    //        var totalChoices = examSubmission.Choices.Count;
//    //        var correctChoices = examSubmission.Choices.Count(choice => IsChoiceCorrect(choice));
//    //        var score = (int)(((double)correctChoices) / totalChoices * 100);
//    //        return score;
//    //    }



//    //    private bool IsChoiceCorrect(ChoiceSubmissionDto choice)
//    //    {
//    //        try
//    //        {
//    //            // Fetch the correct choice from the database based on the ChoiceId
//    //            var correctChoice = _context.Choices
//    //                .Where(c => c.ChoiceId == choice.SelectedChoiceId)
//    //                .FirstOrDefault();

//    //            // Check if the correctChoice exists and is marked as correct
//    //            return correctChoice?.IsCorrect ?? false;
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            // Handle the exception, log it, or return a default value based on your requirements
//    //            // For example, log the exception:
//    //            Console.WriteLine($"Error fetching correct choice: {ex.Message}");
//    //            return false;
//    //        }
//    //    }
//    //}
//    // ExamResultConsumerService.cs


//    //###################################################################################
//    //public class ExamResultConsumerService : BackgroundService
//    //{
//    //    private readonly IRabbitMQService _rabbitMQService;
//    //    private readonly RabbitMQRepository _examResultService;

//    //    public ExamResultConsumerService(IRabbitMQService rabbitMQService, RabbitMQRepository examResultService)
//    //    {
//    //        _rabbitMQService = rabbitMQService;
//    //        _examResultService = examResultService;
//    //    }



//    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    //    {
//    //        using (var channel = _rabbitMQService.Connect())
//    //        {
//    //            channel.QueueDeclare(queue: "exam-submissions", durable: false, exclusive: false, autoDelete: false, arguments: null);

//    //            var consumer = new EventingBasicConsumer(channel);
//    //            consumer.Received += async (model, ea) =>
//    //            {
//    //                var body = ea.Body.ToArray();
//    //                var message = Encoding.UTF8.GetString(body);

//    //                var examSubmission = JsonConvert.DeserializeObject<ExamSubmissionDto>(message);

//    //                var evaluationResult = _examResultService.EvaluateExam(examSubmission);

//    //                var examResultDto = new ExamResultDto
//    //                {
//    //                    StudentExamId = evaluationResult.StudentExamId,
//    //                    Score = evaluationResult.Score,
//    //                    ExamTime = evaluationResult.ExamTime
//    //                };

//    //                _examResultService.SaveExamResult(examResultDto);

//    //                channel.BasicAck(ea.DeliveryTag, false);
//    //            };

//    //            channel.BasicConsume(queue: "exam-submissions", autoAck: false, consumer: consumer);

//    //            await Task.CompletedTask;
//    //        }
//    //    }


//    // Other methods remain unchanged
//    // }

//}
