using StudentSytemData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemService.Interfaces
{
    public interface IAddExamService
    {
       Task AddExamAsync(int subjectId, AddExamDto examDto);

    }
}
