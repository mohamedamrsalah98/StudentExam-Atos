using StudentSystemRepository.Interfaces;
using StudentSystemService.Interfaces;
using StudentSytemData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemService.Service
{
    public class AddSubjectToStudentService : IAddSubjectToStudentService
    {
        private readonly IAddSubjectToStudentRepository _studentSubjectRepository;
        private readonly ISubjectRepository _subjectRepository;

        public AddSubjectToStudentService(IAddSubjectToStudentRepository studentSubjectRepository, ISubjectRepository subjectRepository)
        {
            _studentSubjectRepository = studentSubjectRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<ResultDto<string>> AddSubjectToStudentAsync(string studentId, SubjectDto subjectDto)
        {
            var subject = await _subjectRepository.GetSubjectByNameAsync(subjectDto.SubjectName);
            if (subject == null)
            {
                return ResultDto<string>.Fail("Subject not found.");
            }

            var result = await _studentSubjectRepository.AddSubjectToStudentAsync(studentId, subject.Id);

            if (result != "Subject added to the student successfully.")
            {
                return ResultDto<string>.Fail(result);
            }

            return ResultDto<string>.Success("Subject added to the student successfully");
        }
        public async Task<IEnumerable<SubjectDto>> GetSubjectsForStudentAsync(string studentId)
        {
            var subjects = await _studentSubjectRepository.GetSubjectsForStudentAsync(studentId);
            return subjects.Select(s => new SubjectDto { Id = s.Id, SubjectName = s.SubjectName });
        }
    }
}
