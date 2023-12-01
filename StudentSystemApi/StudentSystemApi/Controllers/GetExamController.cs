using Microsoft.AspNetCore.Mvc;
using StudentSystemService.Interfaces;
using StudentSystemService.RabbitMQ;
using StudentSytemData.Dto;

namespace StudentSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetExamController : ControllerBase
    {
        private readonly IGetExamService _examService;
        private readonly IRabbitMQProducer _rabbitMQProducer;


        public GetExamController(IGetExamService examService,  IRabbitMQProducer rabbitMQProducer)
        {
            _examService = examService;
            _rabbitMQProducer = rabbitMQProducer;
        }



        [HttpPost("random")]
        public async Task<IActionResult> GetRandomExamForStudentAsync([FromBody] StudentSubjectDto examRequest)
        {
            var result = await _examService.GetExamAsync(examRequest.SubjectId, examRequest.StudentId);

            return Ok(result);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitExam([FromBody] ExamSubmissionDto examSubmission)
        {
            try
            {
                var submittedChoices = await _examService.SubmitExamAsync(examSubmission.ExamId, examSubmission.StudentId, examSubmission.Choices);



                if (submittedChoices != null)
                {


                   _rabbitMQProducer.SendProductMessage(submittedChoices);
                    return Ok(submittedChoices);
                }
                else
                {
                    return BadRequest("Failed to submit exam.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-student-exams/{studentId}")]
        public async Task<IActionResult> GetStudentExams(string studentId)
        {
            var studentExams = await _examService.GetStudentExamsAsync(studentId);

            return Ok(studentExams);
        }
    }
}
