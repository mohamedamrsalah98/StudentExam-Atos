using StudentSytemData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemService.Interfaces
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync();
        Task<ResultDto<string>> CreateSubjectAsync(SubjectDto subjectCreate);
        Task<ResultDto<string>> EditSubjectAsync(int subjectId, SubjectDto subjectEdit);
        Task<ResultDto<SubjectDto>> GetSubjectAsync(int id);
        Task<ResultDto<string>> DeleteSubjectAsync(int id);
    }
}
