using StudentSytemData.Dto;
using StudentSytemData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemRepository.Interfaces
{
    public interface IAddExamRepository
    {
        Task AddExamAsync(int subjectId, AddExamDto examDto);

    }
}
