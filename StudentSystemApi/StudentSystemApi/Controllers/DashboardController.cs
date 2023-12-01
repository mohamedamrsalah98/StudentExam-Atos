using Microsoft.AspNetCore.Mvc;
using StudentSystemRepository.Interfaces;
using StudentSytemData.Dto;
using StudentSytemData.Models;

[ApiController]
[Route("api/AllSubjectAllStudent")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardController(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    [HttpGet("student-exam-info")]
    public async Task<ActionResult<List<StudentExamDto>>> GetStudentExamInfo()
    {
        var result = await _dashboardRepository.GetStudentExamInfoAsync();
        return Ok(result);
    }

    [HttpGet("exam-results")]
    public async Task<ActionResult<IEnumerable<ExamResult>>> GetExamResults()
    {
        var examResults = await _dashboardRepository.GetAllExamResultsAsync();
        return Ok(examResults);
    }
}
