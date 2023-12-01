using AutoMapper;
using StudentSystemRepository.Interfaces;
using StudentSystemService.Interfaces;
using StudentSytemData.Dto;




namespace StudentSystemService.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;


        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        async Task<List<StudentDto>> IStudentService.GetUsersInRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                var errorMessage = "RoleName is required.";
                return new List<StudentDto> { new StudentDto { UserName = errorMessage } };
            }

            var student =  await _studentRepository.GetUsersInRoleAsync(roleName);
            if (student.Any())
            {
                return _mapper.Map<List<StudentDto>>(student);
            }
            else
            {
                var message = "No Students added yet.";
                var customResponse = new List<StudentDto>
                 {  new StudentDto { UserName = message }};
                return customResponse;
            }
        }
        public async Task<(bool success, object? message)> UpdateStatus(string userId, StudentStatusDto updateStatusDto)
        {
            var user = await _studentRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return (false, "User not found");
            }

            user.IsActive = updateStatusDto.IsActive;

            var result = await _studentRepository.UpdateAsync(user);

            return result
                ? (true, "Status updated successfully")
                : (false, "Failed to update status");
        }
    }
}



