namespace RabbitMQ.Dto
{
    public class ExamSubmissionDto
    {
        public int ExamId { get; set; }
        public string StudentId { get; set; }
        public List<ChoiceSubmissionDto> Choices { get; set; }
    }
}
