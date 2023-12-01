using StudentSytemData.Dto;
using StudentSytemData.Models;


namespace StudentSystemRepository.Interfaces
{
    public interface IGetExamRepository
    {
        Task<Exam> GetExamAsync(int subjectId);



        Task<IEnumerable<ExamResultDto>> GetStudentExamsAsync(string studentId);
        Task<ExamSubmissionDto> SubmitExamAsync(int examId, string studentId, List<ChoiceSubmissionDto> selectedChoices);
        //ExamResult EvaluateExamAndSave(ExamSubmissionDto examSubmission);






    }
}
