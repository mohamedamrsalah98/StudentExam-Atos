using StudentSytemData.Dto;
using StudentSytemData.Models;


namespace StudentSystemService.Interfaces
{
    public interface IGetExamService
    {
        Task<GetExamDto> GetExamAsync(int subjectId, string studentId);
        Task<ExamSubmissionDto> SubmitExamAsync(int examId, string studentId, List<ChoiceSubmissionDto> selectedChoices);
        Task<IEnumerable<ExamResultDto>> GetStudentExamsAsync(string studentId);






    }
}
