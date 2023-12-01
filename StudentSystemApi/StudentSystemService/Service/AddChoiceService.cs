using AutoMapper;
using StudentSystemRepository.Interfaces;
using StudentSystemService.Interfaces;
using StudentSytemData.Dto;
using StudentSytemData.Models;

namespace StudentSystemService.Service
{
    public class AddChoiceService : IAddChoiceService
    {
        private readonly IAddChoiceRepository _addChoiceRepository;
        private readonly IAddQuestionRepository _addQuestionRepository;
        private readonly IMapper _mapper;

        public AddChoiceService(IAddChoiceRepository addChoiceRepository, IAddQuestionRepository addQuestionRepository, IMapper mapper)
        {
            _addChoiceRepository = addChoiceRepository;
            _addQuestionRepository = addQuestionRepository;
            _mapper = mapper;
        }

        public async Task<string> AddChoicesToQuestionAsync( int questionId, AddChoiceDto choiceDto)
        {

            var existingQuestion = await _addChoiceRepository.GetQuestionByIdAsync(questionId);

            if (existingQuestion == null)
                    return "Question not found.";


            if (await _addChoiceRepository.ChoiceExistsForQuestionAsync(existingQuestion, choiceDto.ChoiceText))
                return "Choice with the same text already exists for this question.";

            var choice = _mapper.Map<Choice>(choiceDto);

            if (existingQuestion.Choices == null)
            {
                existingQuestion.Choices = new List<Choice>();
            }

            existingQuestion.Choices.Add(choice);

            var result = await _addChoiceRepository.AddChoicesToQuestionAsync(existingQuestion, choice);

            return result;
        }

        public async Task<List<AddChoiceDto>> GetChoicesForQuestionAsync(int questionId)
        {
            var existingQuestion = await _addChoiceRepository.GetQuestionByIdAsync(questionId);

            if (existingQuestion == null)
                return new List<AddChoiceDto>();

            var choices = existingQuestion.Choices;
            var choiceDtos = _mapper.Map<List<AddChoiceDto>>(choices);

            return choiceDtos;
        }
    }
}
