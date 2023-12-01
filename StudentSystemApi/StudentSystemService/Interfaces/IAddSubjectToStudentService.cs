using StudentSytemData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemService.Interfaces
{
    public interface IAddSubjectToStudentService
    {
        Task<ResultDto<string>> AddSubjectToStudentAsync(string studentId, SubjectDto subjectDto);
        Task<IEnumerable<SubjectDto>> GetSubjectsForStudentAsync(string studentId);


    }
}
