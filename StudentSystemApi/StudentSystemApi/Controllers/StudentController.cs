using Microsoft.AspNetCore.Mvc;
using StudentSystemService.Interfaces;
using StudentSytemData.Dto;

namespace StudentSystemApi.Controllers
{
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("getUsersInRole")]
        public async Task<IActionResult> GetUsersInRoleAsync(string roleName)
        {
            var result = await _studentService.GetUsersInRoleAsync(roleName);

            return Ok(result);
        }

        [HttpPut("StudentStatus/{userId}")]
        public async Task<IActionResult> UpdateStatus(string userId, [FromBody] StudentStatusDto updateStatusDto)
        {
            var (success, message) = await _studentService.UpdateStatus(userId, updateStatusDto);

            if (success)
            {
                return Ok(message);
            }
            else
            {
                return BadRequest(message);
            }
        }
    }
}
