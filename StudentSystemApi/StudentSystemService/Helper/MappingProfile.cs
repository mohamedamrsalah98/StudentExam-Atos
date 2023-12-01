using AutoMapper;
using StudentSytemData.Dto;
using StudentSytemData.Models;


namespace StudentSystemService.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Subject, SubjectDto>().ReverseMap();
            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<Question, QuestionWithChoicesDto>().ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices)).ReverseMap();
            CreateMap<ApplicationUser, StudentDto>().ReverseMap();
            CreateMap<Choice, ChoiceDto>().ReverseMap();
            CreateMap<Choice, AddChoiceDto>().ReverseMap();
            CreateMap<Exam, AddExamDto>().ReverseMap();
            CreateMap<StudentSubject, StudentSubjectDto>().ReverseMap();
            CreateMap<StudentExam, StudentExamDto>().ReverseMap();
            CreateMap<Exam, GetExamDto>()
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.ExamQuestions.Select(eq => eq.Question)))
            .ReverseMap();






        }
    }
}
