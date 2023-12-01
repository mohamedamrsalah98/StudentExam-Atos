using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using StudentSytemData.Dto;
using StudentSystemService.Interfaces;
using StudentSystemService.Service;

namespace StudentSystemApi.Controllers
{

    [ApiController]
    [Route("api/studentsubjects")]
    public class AddSubjectToStudentController : ControllerBase
    {
        private readonly IAddSubjectToStudentService _studentSubjectService;

        public AddSubjectToStudentController(IAddSubjectToStudentService studentSubjectService)
        {
            _studentSubjectService = studentSubjectService;
        }

        [HttpPost("{studentId}")]
        public async Task<IActionResult> AddSubjectToStudent(string studentId, [FromBody] SubjectDto subjectDto)
        {
            var result = await _studentSubjectService.AddSubjectToStudentAsync(studentId, subjectDto);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetSubjectsForStudent(string studentId)
        {
            var subjects = await _studentSubjectService.GetSubjectsForStudentAsync(studentId);
            return Ok(subjects);
        }
    }







}
