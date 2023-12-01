using Microsoft.AspNetCore.Identity;
using StudentSytemData.Models;
using System.Security.Claims;

namespace StudentSystemRepository.Interfaces
{
    public interface IAuthRepository
    {
        //Task<AuthResult> RegisterAsync(Registeration model);
        //Task<AuthResult> loginAsync(login model);
        //Task<string> AddRoleAsync(RoleModel model);
        //Task<string> DeleteRoleAsync(RoleModel model);

        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> FindByNameAsync(string username);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user);
        Task<List<string>> GetUserRolesAsync(ApplicationUser user);
        Task<string> AddToRoleAsync(ApplicationUser user, string role);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role);
        Task<bool> RoleExistsAsync(string role);
        Task<bool> IsUserInRoleAsync(ApplicationUser user, string role);



    }
}
