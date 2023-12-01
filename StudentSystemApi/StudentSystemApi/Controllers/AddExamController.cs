using Microsoft.AspNetCore.Mvc;
using StudentSystemService.Interfaces;
using StudentSytemData.Dto;

namespace StudentSystemApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AddExamController : ControllerBase
    {
        private readonly IAddExamService _addExamService;

        public AddExamController(IAddExamService addExamService)
        {
            _addExamService = addExamService;
        }

        [HttpPost("{subjectId}")]
        public async Task<IActionResult> AddExamAsync(int subjectId, [FromBody] AddExamDto examDto)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState); 

            //var result = await _addExamService.AddExamAsync(subjectId, examDto);

            //if (!string.IsNullOrEmpty(result))
            //    return BadRequest(result);

            //return Ok(new
            //{
            //    Message = "Exam added successfully."
            //});
            try
            {
                await _addExamService.AddExamAsync(subjectId, examDto);
                return Ok("Exam added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
