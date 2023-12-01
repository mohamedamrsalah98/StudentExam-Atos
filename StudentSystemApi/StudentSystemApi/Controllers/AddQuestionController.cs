using Microsoft.AspNetCore.Mvc;
using StudentSystemService.Interfaces;
using StudentSystemService.Service;
using StudentSytemData.Dto;

namespace StudentSystemApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AddQuestionController : ControllerBase
    {
        private readonly IAddQuestionService _addquestionService;

        public AddQuestionController(IAddQuestionService addquestionService)
        {
            _addquestionService = addquestionService;
        }

        [HttpPost("{subjectId}")]
        public async Task<IActionResult> AddQuestionToSubject([FromBody] QuestionDto questionDto, int subjectId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _addquestionService.AddQuestionToSubjectAsync(subjectId, questionDto);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(new
            {
                Message = "Question added successfully."
            });
        }
        [HttpGet("GetQuestionsForSubject/{subjectId}")]
        public async Task<IActionResult> GetQuestionsForSubject(int subjectId)
        {
            var questions = await _addquestionService.GetQuestionsForSubjectAsync(subjectId);

            return Ok(questions);
        }

        [HttpGet("GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            try
            {
                var questions = await _addquestionService.GetAllQuestionsAsync();
                return Ok(questions);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
