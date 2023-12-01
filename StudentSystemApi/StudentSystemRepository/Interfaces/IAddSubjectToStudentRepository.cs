using StudentSytemData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemRepository.Interfaces
{
    public interface IAddSubjectToStudentRepository
    {
        Task<string> AddSubjectToStudentAsync(string studentId, int subjectId);
        Task<IEnumerable<Subject>> GetSubjectsForStudentAsync(string studentId);


    }
}
