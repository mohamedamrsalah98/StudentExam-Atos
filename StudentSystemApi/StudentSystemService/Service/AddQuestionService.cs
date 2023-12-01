using AutoMapper;
using StudentSystemRepository.Interfaces;
using StudentSystemService.Interfaces;
using StudentSytemData.Dto;
using StudentSytemData.Models;


namespace StudentSystemService.Service
{
    public class AddQuestionService : IAddQuestionService
    {
        private readonly IAddQuestionRepository _addQuestionsRepository;
        private readonly IMapper _mapper;

        public AddQuestionService(IAddQuestionRepository addQuestionsRepository,IMapper mapper)
        {
            _addQuestionsRepository = addQuestionsRepository;
            _mapper = mapper;
        }

        public async Task<string> AddQuestionToSubjectAsync(int subjectId, QuestionDto questionDto)
        {
            var existingSubject = await _addQuestionsRepository.GetSubjectByIdAsync(subjectId);

            if (existingSubject == null)
                return "Subject not found.";

            if (await _addQuestionsRepository.QuestionExistsInSubjectAsync(existingSubject, questionDto.QuestionText))
            {
                return "Question already exists in the subject.";
            }



            var question = _mapper.Map<Question>(questionDto);


            var result = await _addQuestionsRepository.AddQuestionsToSubjectAsync(existingSubject, question);

            return result;
        }
        public async Task<List<QuestionDto>> GetQuestionsForSubjectAsync(int subjectId)
        {
            var questions = await _addQuestionsRepository.GetQuestionsForSubjectAsync(subjectId);

            return _mapper.Map<List<QuestionDto>>(questions);
        }

        public async Task<List<QuestionDto>> GetAllQuestionsAsync()
        {
            var questions = await _addQuestionsRepository.GetAllQuestionsAsync();
            return _mapper.Map<List<QuestionDto>>(questions);
        }
    }
}
