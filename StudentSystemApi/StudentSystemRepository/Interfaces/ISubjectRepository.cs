using StudentSytemData.Models;


namespace StudentSystemRepository.Interfaces
{
    public interface ISubjectRepository
    {
        Task<ICollection<Subject>> GetAllSubjectsAsync();
        Task<bool> CreateSubjectAsync(Subject subject);
        Task<bool> SubjectExistsAsync(string subjectName);
        Task<bool> SubjectExistsAsync(int id);
        Task<bool> UpdateSubjectAsync(Subject subject);
        Task<bool> DeleteSubjectAsync(int id);
        Task<Subject> GetSubjectByIdAsync(int subjectId);
        Task<Subject> GetSubjectByNameAsync(string subjectName);

    }
}
