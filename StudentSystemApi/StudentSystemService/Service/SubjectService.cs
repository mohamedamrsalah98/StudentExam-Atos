using AutoMapper;
using StudentSystemRepository.Interfaces;
using StudentSystemService.Interfaces;
using StudentSytemData.Dto;
using StudentSytemData.Models;


namespace StudentSystemService.Service
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;


        public SubjectService(ISubjectRepository subjectRepository, IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;

        }

        public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
        {
            var subjects = await _subjectRepository.GetAllSubjectsAsync();

            if (subjects.Any())
            {
                return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
            }
            else
            {
                var message = "No subjects added yet.";
                var customResponse = new List<SubjectDto>
                 {  new SubjectDto { SubjectName = message }};
                return customResponse;
            }
        }

        public async Task<ResultDto<string>> CreateSubjectAsync(SubjectDto subjectCreate)
        {
            if (subjectCreate == null)
            {
                return ResultDto<string>.Fail("Invalid subject data");
            }

            var subjectExists = await _subjectRepository.SubjectExistsAsync(subjectCreate.SubjectName);


            if (subjectExists)
            {
                return ResultDto<string>.Fail("Subject already exists");
            }

            var subjectMap = _mapper.Map<Subject>(subjectCreate);

            if (await _subjectRepository.CreateSubjectAsync(subjectMap))
            {
                return ResultDto<string>.Success("Some data", "Successfully created");
            }

            return ResultDto<string>.Fail("Something went wrong while saving");
        }


        public async Task<ResultDto<string>> EditSubjectAsync(int subjectId, SubjectDto subjectEdit)
        {
            var existingSubject = await _subjectRepository.GetSubjectByIdAsync(subjectId);

            if (existingSubject == null)
            {
                return ResultDto<string>.Fail("Subject not found");
            }

            if (subjectEdit == null)
            {
                return ResultDto<string>.Fail("Invalid subject data");
            }

            var subjectExists = await _subjectRepository.SubjectExistsAsync(subjectEdit.SubjectName);

            if (subjectExists && subjectEdit.SubjectName != existingSubject.SubjectName)
            {
                return ResultDto<string>.Fail("Subject name already exists");
            }

            existingSubject.SubjectName = subjectEdit.SubjectName;

            if (await _subjectRepository.UpdateSubjectAsync(existingSubject))
            {
                return ResultDto<string>.Success("Success message", "successfully updated");

            }

            return ResultDto<string>.Fail("Something went wrong while saving");
        }
        public async Task<ResultDto<SubjectDto>> GetSubjectAsync(int id)
        {
            var subject = await _subjectRepository.GetSubjectByIdAsync(id);

            if (subject == null)
            {
                return ResultDto<SubjectDto>.Fail("Subject not found");
            }

            var subjectDto = _mapper.Map<SubjectDto>(subject);
            return ResultDto<SubjectDto>.Success(subjectDto);
        }

        public async Task<ResultDto<string>> DeleteSubjectAsync(int id)
        {
            var exists = await _subjectRepository.SubjectExistsAsync(id);


            if (!exists)
            {
                return ResultDto<string>.Fail("Subject not found");
            }

            var success = await _subjectRepository.DeleteSubjectAsync(id);

            if (success)
            {
                return ResultDto<string>.Success("Success message", "Subject deleted successfully");
            }

            return ResultDto<string>.Fail("Something went wrong while deleting the subject");
        }
    }
}
