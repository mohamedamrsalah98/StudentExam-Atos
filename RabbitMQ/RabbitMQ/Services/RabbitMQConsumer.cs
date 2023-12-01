using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Interfaces;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Dto;

namespace RabbitMQ.Services
{
    public class RabbitMQConsumer : BackgroundService, IRabbitMQConsumer
    {
        private readonly IServiceProvider _serviceProvider;
        private IModel _channel;

        public RabbitMQConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"

            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.QueueDeclare(
                queue: "Exam",
                durable: false,
                exclusive: false,
                autoDelete: true, 
                arguments: null
            );
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var examService = scope.ServiceProvider.GetRequiredService<IExamService>();

                    var examSubmission = JsonConvert.DeserializeObject<ExamSubmissionDto>(message);

                    examService.EvaluateExamAndSave(examSubmission);

                    Console.WriteLine($" [x] Received '{message}'");
                }
            };

            _channel.BasicConsume(queue: "Exam", autoAck: true, consumer: consumer);

            Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            base.Dispose();
        }
    }
}
