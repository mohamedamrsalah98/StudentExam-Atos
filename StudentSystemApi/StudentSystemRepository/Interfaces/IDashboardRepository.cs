using StudentSytemData.Dto;
using StudentSytemData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemRepository.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<AllStudentAllSubjectDto>> GetStudentExamInfoAsync();
        Task<IEnumerable<ExamResult>> GetAllExamResultsAsync();


    }
}
