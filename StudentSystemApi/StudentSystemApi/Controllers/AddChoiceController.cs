using Microsoft.AspNetCore.Mvc;
using StudentSystemService.Interfaces;
using StudentSytemData.Dto;

namespace StudentSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddChoiceController : ControllerBase
    {
        private readonly IAddChoiceService _addChoiceService;

        public AddChoiceController(IAddChoiceService addChoiceService)
        {
            _addChoiceService = addChoiceService;
        }

        [HttpPost("{questionId}")]
        public async Task<IActionResult> AddChoicesToQuestionAsync(int questionId, [FromBody] AddChoiceDto choiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _addChoiceService.AddChoicesToQuestionAsync( questionId, choiceDto);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(new
            {
                Message = "Choices added successfully."
            });
        }
        [HttpGet("GetChoicesForQuestion/{questionId}")]
        public async Task<IActionResult> GetChoicesForQuestion(int questionId)
        {
            var choices = await _addChoiceService.GetChoicesForQuestionAsync(questionId);
            return Ok(choices);
        }
    }
}
