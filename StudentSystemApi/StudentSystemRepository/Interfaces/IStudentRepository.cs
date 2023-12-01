using StudentSytemData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemRepository.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<ApplicationUser>> GetUsersInRoleAsync(string roleName);
        Task<bool> UpdateAsync(ApplicationUser user);
        Task<ApplicationUser> GetByIdAsync(string userId);


    }
}
