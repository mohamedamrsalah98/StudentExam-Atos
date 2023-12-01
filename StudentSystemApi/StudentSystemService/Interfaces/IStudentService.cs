using StudentSytemData.Dto;


namespace StudentSystemService.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentDto>> GetUsersInRoleAsync(string roleName);
        Task<(bool success, object? message)> UpdateStatus(string userId, StudentStatusDto updateStatusDto);


    }
}
