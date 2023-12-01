using StudentSytemData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemService.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegistrationAsync(Registeration model);
        Task<AuthResult> LoginAsync(login model);
        Task<string> AddRoleAsync(RoleModel model);
        Task<string> DeleteRoleAsync(RoleModel model);


    }
}
