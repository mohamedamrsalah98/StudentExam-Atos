using AutoMapper;
using StudentSystemRepository.Interfaces;
using StudentSystemService.Interfaces;
using StudentSytemData.Data;
using StudentSytemData.Dto;
using StudentSytemData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemService.Service
{
    public class AddExamService : IAddExamService
    {
        private readonly IAddExamRepository _addExamRepository;
        private readonly IAddQuestionRepository _addQuestionRepository;
        private readonly DataContext _context;

        private readonly IMapper _mapper;

        public AddExamService(IAddExamRepository addExamRepository, IAddQuestionRepository addQuestionRepository, IMapper mapper, DataContext context)
        {
            _addExamRepository = addExamRepository;
            _addQuestionRepository = addQuestionRepository;
            _mapper = mapper;
            _context = context;

        }

        public async Task AddExamAsync(int subjectId, AddExamDto examDto)
        {
            await _addExamRepository.AddExamAsync(subjectId, examDto);

        }
    }
}
