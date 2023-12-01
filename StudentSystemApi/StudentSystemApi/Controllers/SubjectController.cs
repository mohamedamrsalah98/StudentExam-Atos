using Microsoft.AspNetCore.Mvc;
using StudentSystemService.Interfaces;
using StudentSystemService.Service;
using StudentSytemData.Dto;
using StudentSytemData.Models;

namespace StudentSystemApi.Controllers
{
    [Route("api/subjects")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Subject>))]
        public async Task<IActionResult> GetSubjects()
        {
            var subjectDtos = await _subjectService.GetAllSubjectsAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(subjectDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectDto subjectCreate)
        {
            var result = await _subjectService.CreateSubjectAsync(subjectCreate);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("CreateSubject", result.Message);
                return BadRequest(ModelState);
            }

            return Ok(result.Message);
        }

        [HttpPut("{subjectId}")]
        public async Task<IActionResult> EditSubject(int subjectId, [FromBody] SubjectDto subjectDto)
        {
            var result = await _subjectService.EditSubjectAsync(subjectId, subjectDto);


            if (!result.IsSuccess)
            {
                ModelState.AddModelError("EditSubject", result.Message);
                return BadRequest(ModelState);
            }

            return Ok(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubject(int id)
        {
            var subject = await _subjectService.GetSubjectAsync(id);

            if (subject == null)
            {
                return NotFound("Subject not found");
            }

            return Ok(subject);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var result = await _subjectService.DeleteSubjectAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound("Subject not found");
            }

            return Ok("Subject deleted successfully");
        }

    }
}
