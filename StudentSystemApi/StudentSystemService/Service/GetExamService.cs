using AutoMapper;
using StudentSystemRepository.Interfaces;
using StudentSystemService.Interfaces;
using StudentSytemData.Dto;
using StudentSytemData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemService.Service
{
    public class GetExamService : IGetExamService
    {
        private readonly IGetExamRepository _getExamRepository;
        private readonly IMapper _mapper;


        public GetExamService(IGetExamRepository getExamRepository, IMapper mapper)
        {
            _getExamRepository = getExamRepository;
            _mapper = mapper;
        }

        public async Task<GetExamDto> GetExamAsync(int subjectId,string studentId)
        {
            var exams = await _getExamRepository.GetExamAsync(subjectId);

            var examDtos = _mapper.Map<GetExamDto>(exams);

            return examDtos;

        }

        public async Task<ExamSubmissionDto> SubmitExamAsync(int examId, string studentId, List<ChoiceSubmissionDto> selectedChoices)
        {

            return await _getExamRepository.SubmitExamAsync(examId, studentId, selectedChoices);
        }


        public async Task<IEnumerable<ExamResultDto>> GetStudentExamsAsync(string studentId)
        {
            return await _getExamRepository.GetStudentExamsAsync(studentId);
        }

    }

}

